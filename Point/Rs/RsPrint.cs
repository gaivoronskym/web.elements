using System.Text.RegularExpressions;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Rs;

public sealed class RsPrint : RsWrap, IText
{
    private readonly Regex _patternFirstLine = new Regex("HTTP/1\\.1 \\d{3} [a-zA-Z- ]+", RegexOptions.Compiled);
    private readonly Regex _patternOtherLines = new Regex("/([\\w-]+): (.*)/g", RegexOptions.Compiled);
    
    public RsPrint(IResponse origin) : base(origin)
    {
    }

    public string AsString()
    {
        return Print();
    }

    public string Print()
    {
        using Stream stream = new MemoryStream();
        Print(stream);
        stream.Position = 0;

        return new TextOf(
            new InputOf(
                stream
            )
        ).AsString();
    }

    public void Print(Stream output)
    {
        PrintHead(output);
        PrintBody(output);
    }

    public void PrintBody(Stream output)
    {
        try
        {
            var bytes = new BytesOf(
                new InputOf(Body)
            ).AsBytes();
            
            output.Write(
                bytes,
                0,
                bytes.Length
            );
        }
        catch (Exception e)
        {
            output.Close();
        }
    }

    public void PrintHead(Stream output)
    {
        int index = 0;
        
        foreach (var line in Head())
        {
            if (index == 0 && !_patternFirstLine.IsMatch(line))
            {
                throw new ArgumentException(
                    string.Format(
                        @"First line of HTTP Response ""{0}"" does not match ""{1}"" regular expression, but it should, according to RFC 7230",
                        line,
                        _patternFirstLine
                    )
                );
            }

            //need to check all others
            /*if (index > 0 && !_patternOtherLines.IsMatch(line))
            {
                throw new ArgumentException(
                    string.Format(
                        @"Header line {0} HTTP Response ""{1}"" does not match ""{2}"" regular expression, but it should, according to RFC 7230",
                        index,
                        line,
                        _patternFirstLine
                    )
                );
            }*/

            var text = new TextOf(line);

            var expression = new Or(
                new StartsWith(text, "HTTP"),
                new StartsWith(text, "Content-Length"),
                new StartsWith(text, "Content-Type")
            );
            
            if (expression.Value())
            {
                output.Write(new BytesOf(
                        new TextOf(line + Environment.NewLine)
                    ).AsBytes()
                );
            }
            else
            {
                output.Write(new BytesOf(
                        new TextOf(line)
                    ).AsBytes()
                );
            }

            ++index;
        }

        output.Write(
            new BytesOf(
                new TextOf(Environment.NewLine)
            ).AsBytes()
        );
    }
}