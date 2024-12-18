using System.Text;
using System.Text.RegularExpressions;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rs;

public sealed class RsPrint : RsWrap, IText
{
    private static readonly Regex first = new Regex("HTTP/1\\.1 \\d{3} [a-zA-Z- ]+", RegexOptions.Compiled);
    private static readonly Regex others = new Regex("([\\w-]+): (.*)+", RegexOptions.Compiled);
    
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

    public string PrintBody()
    {
        using Stream stream = new MemoryStream();
        PrintBody(stream);
        stream.Position = 0;

        return new TextOf(
            new InputOf(
                stream
            )
        ).AsString();
    }

    public string PrintHead()
    {
        using Stream stream = new MemoryStream();
        PrintHead(stream);
        stream.Position = 0;

        return new TextOf(
            new InputOf(
                stream
            )
        ).AsString();
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
        catch (Exception)
        {
            output.Close();
        }
    }

    public void PrintHead(Stream output)
    {
        var pos = 0;
        const string eol = "\r\n";
        foreach (var line in Head())
        {
            if (pos == 0 && !first.IsMatch(line))
            {
                throw new ArgumentException(
                    string.Format(
                        @"First line of HTTP Response ""{0}"" does not match ""{1}"" regular expression, but it should, according to RFC 7230",
                        line,
                        first
                    )
                );
            }

            if (pos > 0 && !others.IsMatch(line))
            {
                throw new ArgumentException(
                    string.Format(
                        @"Header line {0} HTTP Response ""{1}"" does not match ""{2}"" regular expression, but it should, according to RFC 7230",
                        pos,
                        line,
                        others
                    )
                );
            }
            
            output.Write(Encoding.Default.GetBytes(line));
            output.Write(Encoding.Default.GetBytes(eol));

            ++pos;
        }

        output.Write(
            new BytesOf(
                new TextOf(Environment.NewLine)
            ).AsBytes()
        );
    }
}