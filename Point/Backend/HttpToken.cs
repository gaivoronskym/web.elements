using System.Buffers;
using System.Text;

namespace Point.Backend;

public class BufferedRequest : IBufferedRequest
{
    private readonly ReadOnlySequence<byte> _buffer;

    public BufferedRequest(ReadOnlySequence<byte> buffer)
    {
        _buffer = buffer;
    }

    public string Token(char delimiter)
    {
        var delimiterByte = (byte)delimiter;
        
    }

    public IBufferedRequest WithSkipped(int bytes)
    {
        return new BufferedRequest(_buffer.Slice(bytes));
    }
    
    private string GetString()
    {
        return string.Create((int)_buffer.Length, _buffer, (span, sequence) =>
        {
            foreach (var segment in sequence)
            {
                Encoding.ASCII.GetChars(segment.Span, span);
                span = span[segment.Length..];
            }
        });
    }
}