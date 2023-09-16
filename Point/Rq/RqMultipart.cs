using System.Buffers;
using System.Net;
using System.Text.RegularExpressions;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public class RqMultipart : IRqMultipart
{
    private readonly IRequest _origin;
    private readonly IDictionary<string, IList<IRequest>> _map;

    private readonly Regex _multipartRegex = new Regex(@"multipart/form-data; boundary=(?<boundary>[^/+d]+)", RegexOptions.Compiled);

    public RqMultipart(IRequest origin)
    {
        _origin = origin;
        _map = new Dictionary<string, IList<IRequest>>();
    }

    public IEnumerable<IRequest> Part(string name)
    {
        if (_map.Count == 0)
        {
            var res = Requests(_origin);
            foreach (var item in res)
            {
                _map.Add(item);
            }
        }

        if (!_map.ContainsKey(name))
        {
            throw new HttpRequestException(
                    $"Bad Request. Key '{name}' is missing."
            );
        }
        
        return _map[name];
    }

    public IEnumerable<string> Names()
    {
        return _map.Keys;
    }

    public IEnumerable<string> Head()
    {
        return _origin.Head();
    }

    public Stream Body()
    {
        return _origin.Body();
    }

    private IDictionary<string, IList<IRequest>> Requests(IRequest req)
    {
        string header = new RqHeaders(req).Headers()["Content-Type"];

        if (!_multipartRegex.IsMatch(header))
        {
            throw new HttpRequestException(
                "RqMultipart can can only parse multipart/form-data",
                null,
                HttpStatusCode.BadRequest
            );
        }

        string boundary = _multipartRegex.Matches(header).First().Groups["boundary"].Value;

        IList<IRequest> requests = new List<IRequest>();

        var delimiter = new BytesOf(
            string.Concat("--", boundary, "\r\n")
        ).AsBytes();

        foreach (ReadOnlySequence<byte> chunk in Chunk(Body(), delimiter))
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

        List<string> head = new List<string>();

        Regex regex = new Regex(@"(?<type>[^w]+): (?<value>[^w]+)");
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
        var bufferSize = 65536;
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

    /*public static async IAsyncEnumerable<ReadOnlySequence<byte>> ReadPipeAsync(PipeReader reader,
        ReadOnlyMemory<byte> delimiter,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (true)
        {
            // Read from the PipeReader.
            ReadResult result = await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            ReadOnlySequence<byte> buffer = result.Buffer;

            while (TryReadChunk(ref buffer, delimiter.Span, out ReadOnlySequence<byte> chunk))
                yield return chunk;

            // Tell the PipeReader how many bytes are read.
            // This is essential because the Pipe will release last used buffer segments that are not longer in use.
            reader.AdvanceTo(buffer.Start, buffer.End);

            // Take care of the complete notification and return the last buffer. UPDATE: Corrected issue 2/.
            if (result.IsCompleted)
            {
                yield return buffer;
                break;
            }
        }

        await reader.CompleteAsync().ConfigureAwait(false);
    }

    private static bool TryReadChunk(ref ReadOnlySequence<byte> buffer, ReadOnlySpan<byte> delimiter,
        out ReadOnlySequence<byte> chunk)
    {
        // Search the buffer for the first byte of the delimiter.
        SequencePosition? position = buffer.PositionOf(delimiter[0]);

        // If no occurence was found or the next bytes of the data in the buffer does not match the delimiter, return false.
        // UPDATE: Corrected issue 3/.
        if (position is null || !buffer.Slice(position.Value, delimiter.Length).FirstSpan.StartsWith(delimiter))
        {
            chunk = default;
            return false;
        }

        // Return the calculated chunk and update the buffer to cut the start.
        chunk = buffer.Slice(0, position.Value);
        buffer = buffer.Slice(buffer.GetPosition(delimiter.Length, position.Value));
        return true;
    }*/
}