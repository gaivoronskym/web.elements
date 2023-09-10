using System.Net.Sockets;
using System.Text.RegularExpressions;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Rs;

public class RsPrint : RsWrap, IText
{
    private readonly Regex _patternFirstLine = new Regex("HTTP/1\\.1 \\d{3} [a-zA-Z- ]+", RegexOptions.Compiled);
    private readonly Regex _patternOtherLines = new Regex("/([\\w-]+): (.*)/g", RegexOptions.Compiled);
    
    public RsPrint(IResponse origin) : base(origin)
    {
    }

    public string AsString()
    {
        throw new NotImplementedException();
    }

    public void Print(NetworkStream output)
    {
        PrintHead(output);
        PrintBody(output);
    }

    public void PrintBody(NetworkStream output)
    {
        try
        {
            output.Write(
                new BytesOf(
                    new InputOf(Body)
                ).AsBytes()
            );
        }
        catch (Exception e)
        {
            output.Close();
        }
    }

    public void PrintHead(NetworkStream output)
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