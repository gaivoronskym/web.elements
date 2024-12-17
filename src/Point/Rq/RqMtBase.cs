using System.Buffers;
using System.Net;
using System.Text.RegularExpressions;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public sealed class RqMtBase : IRqMultipart
{
    private readonly IRqHeaders origin;
    private readonly IDictionary<string, IList<IRequest>> map;

    private readonly Regex multipartRegex =
        new Regex(@"multipart/form-data; boundary=(?<boundary>[^/+d]+)", RegexOptions.Compiled);

    public RqMtBase(IRequest origin)
    {
        this.origin = new IRqHeaders.Base(origin);
        this.map = new Dictionary<string, IList<IRequest>>();
    }

    public IEnumerable<IRequest> Part(string name)
    {
        if (map.Count == 0)
        {
            var res = Requests(origin);
            foreach (var item in res)
            {
                map.Add(item);
            }
        }

        if (!map.ContainsKey(name))
        {
            throw new HttpRequestException(
                $"Bad Request. Key '{name}' is missing."
            );
        }

        return map[name];
    }

    public IEnumerable<string> Names()
    {
        return map.Keys;
    }

    public IEnumerable<string> Head()
    {
        return origin.Head();
    }

    public Stream Body()
    {
        return origin.Body();
    }

    private IDictionary<string, IList<IRequest>> Requests(IRequest req)
    {
        var header = this.origin.Header("Content-Type")[0];

        if (!multipartRegex.IsMatch(header))
        {
            throw new HttpRequestException(
                "RqMultipart can only parse multipart/form-data",
                null,
                HttpStatusCode.BadRequest
            );
        }

        var boundary = multipartRegex.Matches(header).First().Groups["boundary"].Value;

        IList<IRequest> requests = new List<IRequest>();

        var delimiter = new BytesOf(
            string.Concat("--", boundary, "\r\n")
        ).AsBytes();

        foreach (var chunk in Chunk(Body(), delimiter))
        {
            requests.Add(Make(chunk));
        }

        return AsMap(requests);
    }

    private IRequest Make(ReadOnlySequence<byte> bytes)
    {
        var delimiter = new BytesOf(
            new TextOf("\r\n")
        ).AsBytes();

        var endOfHeaderPosition = bytes.FirstSpan.IndexOf(delimiter);

        var head = new List<string>();

        var regex = new Regex(@"(?<type>[^w]+): (?<value>[^w]+)");
        while (endOfHeaderPosition >= 0)
        {
            var header = new TextOf(
                new InputOf(
                    bytes.Slice(0, endOfHeaderPosition).ToArray()
                )
            ).AsString();

            if (!regex.IsMatch(header))
            {
                break;
            }

            if (!string.IsNullOrEmpty(header.Trim()))
            {
                head.Add(header);
            }

            bytes = bytes.Slice(endOfHeaderPosition + delimiter.Length);
            endOfHeaderPosition = bytes.FirstSpan.IndexOf(delimiter);
        }

        bytes = bytes.Slice(delimiter.Length, bytes.Length - delimiter.Length * 2);

        var stream = new MemoryStream();
        var body = bytes.Slice(endOfHeaderPosition, bytes.End).ToArray();

        stream.Write(body, 0, body.Length);
        stream.Flush();
        stream.Position = 0;

        return new RequestOf(
            head,
            stream
        );
    }

    private IDictionary<string, IList<IRequest>> AsMap(IList<IRequest> reqs)
    {
        IDictionary<string, IList<IRequest>> map = new Dictionary<string, IList<IRequest>>();

        foreach (var req in reqs)
        {
            var singlePart = new RqSinglePart(req);

            var name = singlePart.PartName();

            if (!map.ContainsKey(name))
            {
                map.Add(name, new List<IRequest>());
            }

            map[name].Add(req);
        }

        return map;
    }

    private IEnumerable<ReadOnlySequence<byte>> Chunk(Stream stream, byte[] delimiter)
    {
        var bufferSize = 65536 * 3;
        var buffer = new byte[bufferSize];
        var size = bufferSize;
        var offset = 0;
        var position = stream.Position;
        var minChunkSize = 0;
        var nextChunk = Array.Empty<byte>();

        while (true)
        {
            var bytesRead = stream.Read(buffer, offset, size);

            // when no bytes are read -- the string could not be found
            if (bytesRead <= 0)
                break;

            // when less then size bytes are read, we need to slice the buffer to prevent reading of "previous" bytes
            ReadOnlySpan<byte> ro = buffer;
            if (bytesRead < size)
                ro = ro.Slice(0, offset + bytesRead);

            // check if we can find our search bytes in the buffer
            var i = ro.IndexOf(delimiter);
            if (i > -1 && // we found something
                i <= bytesRead && //i <= r  -- we found something in the area that was read (at the end of the buffer, the last values are not overwritten). i = r if the delimiter is at the end of the buffer
                nextChunk.Length + (i + delimiter.Length - offset) >=
                minChunkSize) //the size of the chunk that will be made is large enough
            {
                var chunk = buffer[offset..(i + delimiter.Length)];

                if (!chunk.SequenceEqual(delimiter))
                {
                    chunk = chunk.Take(chunk.Length - delimiter.Length).ToArray();
                    yield return new ReadOnlySequence<byte>(nextChunk.Concat(chunk).ToArray());
                }

                nextChunk = Array.Empty<byte>();

                offset = 0;
                size = bufferSize;
                position += i + delimiter.Length;
                stream.Position = position;
                continue;
            }
            else if (stream.Position == stream.Length)
            {
                // we re at the end of the stream
                var chunk = buffer[offset..(bytesRead - delimiter.Length - 2 + offset)]; //return the bytes read

                yield return new ReadOnlySequence<byte>(nextChunk.Concat(chunk).ToArray());

                break;
            }

            // the stream is not finished. Copy the last 2 bytes to the beginning of the buffer and set the offset to fill the buffer as of byte 3

            nextChunk = nextChunk.Concat(buffer[offset..buffer.Length]).ToArray();

            offset = delimiter.Length;
            size = bufferSize - offset;
            Array.Copy(buffer, buffer.Length - offset, buffer, 0, offset);
            position += bufferSize - offset;
        }
    }
}