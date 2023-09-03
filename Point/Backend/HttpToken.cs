using System.Buffers;
using System.IO.Pipelines;
using System.Text;

namespace Point.Backend;

public class HttpToken : IHttpToken
{
    private readonly PipeReader _pipe;
    private readonly ReadOnlySequence<byte> _buffer;

    public HttpToken(PipeReader pipe, ReadOnlySequence<byte> buffer)
    {
        _pipe = pipe;
        _buffer = buffer;
    }

    public string AsString(char delimiter)
    {
        var delimiterByte = (byte)delimiter;
        var position = _buffer.PositionOf(delimiterByte);

        if (position is null)
        {
            return string.Empty;
        }

        return Parse(_buffer.Slice(0, position.Value));
    }

    public IHttpToken Skip(char delimiter)
    {
        var delimiterByte = (byte)delimiter;
        var position = _buffer.PositionOf(delimiterByte);

        if (!position.HasValue)
        {
            throw new NullReferenceException();
        }

        ReadOnlySequence<byte> tempBuffer = _buffer.Slice(position.Value);
        _pipe.AdvanceTo(tempBuffer.Start);

        return new HttpToken(
            _pipe,
            tempBuffer
        );
    }

    public IHttpToken SkipNext(byte length)
    {
        ReadOnlySequence<byte> tempBuffer = _buffer.Slice(length);
        _pipe.AdvanceTo(tempBuffer.Start);

        return new HttpToken(
            _pipe,
            tempBuffer
        );
    }

    public bool NextIs(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var value = Parse(_buffer.Slice(0, token.Length));

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