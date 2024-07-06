using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Backend.Http;

public class HttpToken : IHttpToken
{
    private readonly PipeReader pipe;
    private readonly ReadOnlySequence<byte> buffer;

    public HttpToken(PipeReader pipe, ReadOnlySequence<byte> buffer)
    {
        this.pipe = pipe;
        this.buffer = buffer;
    }

    public string AsString(char delimiter)
    {
        var delimiterByte = (byte)delimiter;
        var position = buffer.PositionOf(delimiterByte);

        if (position is null)
        {
            return string.Empty;
        }

        return Parse(buffer.Slice(0, position.Value));
    }

    public Stream Stream()
    {
        if (buffer.Length == 0)
        {
            return new InputOf(
                new TextOf(string.Empty)
            ).Stream();
        }

        return new InputOf(
            buffer.ToArray()
        ).Stream();
    }

    public IHttpToken Skip(char delimiter)
    {
        var delimiterByte = (byte)delimiter;
        var position = buffer.PositionOf(delimiterByte);
        
        if (!position.HasValue)
        {
            throw new NullReferenceException();
        }

        var tempBuffer = buffer.Slice(position.Value);
        pipe.AdvanceTo(tempBuffer.Start);

        return new HttpToken(
            pipe,
            tempBuffer
        );
    }

    public IHttpToken SkipNext(byte length)
    {
        var tempBuffer = buffer.Slice(length);
        pipe.AdvanceTo(tempBuffer.Start);

        return new HttpToken(
            pipe,
            tempBuffer
        );
    }

    public bool NextIs(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var value = Parse(buffer.Slice(0, token.Length));

        return value.Equals(token);
    }

    private string Parse(ReadOnlySequence<byte> buffer)
    {
        return string.Create((int)buffer.Length, buffer, (span, sequence) =>
        {
            foreach (var segment in sequence)
            {
                Encoding.ASCII.GetChars(segment.Span, span);
                span = span[segment.Length..];
            }
        });
    }
}