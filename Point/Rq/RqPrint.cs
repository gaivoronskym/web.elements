using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public sealed class RqPrint : RqWrap, IRqPrint
{
    private readonly IText _text;
    
    public RqPrint(IRequest origin) : base(origin)
    {
        _text = new TextOf(() =>
        {
            using Stream stream = new MemoryStream();
            Print(stream);
            stream.Position = 0;
            return new TextOf(stream).AsString();
        });
    }

    public void Print(Stream output)
    {
        PrintHead(output);
        PrintBody(output);
    }

    public void PrintHead(Stream output)
    {
        foreach (var line in Head())
        {
            var lineBytes = new BytesOf(
                new TextOf(
                    line + Environment.NewLine
                )
            ).AsBytes();
            
            output.Write(lineBytes, 0, lineBytes.Length);
        }
        
        var bytes = new BytesOf(
            new TextOf(
                Environment.NewLine
            )
        ).AsBytes();
        
        output.Write(bytes, 0, bytes.Length);
    }

    public void PrintBody(Stream output)
    {
        var input = Body();
        
        var buffer = new byte[4096];

        while (true)
        {
            var bytes = input.Read(buffer);
            if (bytes <= 0)
            {
                break;
            }
            
            output.Write(buffer, 0, bytes);
        }
    }

    public string AsString()
    {
        return _text.AsString();
    }
}