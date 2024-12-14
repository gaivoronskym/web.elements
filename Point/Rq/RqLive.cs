using System.Net;
using Point.Exceptions;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public sealed class RqLive : RqWrap
{
    public RqLive(Stream input)
        : base(
            RqLive.Parse(input)
        )
    {
    }

    private static IRequest Parse(Stream input)
    {
        bool eof = true;
        IList<string> head = new List<string>();
        IList<char> output = new List<char>();
        IOpt<int> data = new IOpt<int>.Empty();

        data = RqLive.Data(input, data, false);

        while (data.Value() > 0)
        {
            eof = false;
            if (data.Value() == '\r')
            {
                RqLive.CheckLineFeed(input, output, head.Count + 1);
                if (output.Count == 0)
                {
                    break;
                }

                data = new Opt<int>(input.ReadByte());
                IOpt<string> header = RqLive.NewHeader(data, output);
                if (header.Has())
                {
                    head.Add(header.Value());
                }
                
                data = RqLive.Data(input, data, false);
                continue;
            }

            output.Add(RqLive.LegalCharacter(data, input, head.Count + 1));
            data = RqLive.Data(input, new IOpt<int>.Empty(), true);
        }

        if (eof)
        {
            throw new IOException("empty request");
        }
        
        return new RequestOf(head, input);
    }

    private static void CheckLineFeed(Stream input, IEnumerable<char> characters, int position)
    {
        if (input.ReadByte() != '\n')
        {
            throw new HttpException(
                HttpStatusCode.BadRequest,
                new Formatted(
                    "there is no LF after CR in header, line #{0}: \"{1}\"",
                    position,
                    new string(characters.ToArray())
                ).AsString()
            );
        }
    }

    private static IOpt<string> NewHeader(IOpt<int> data, IList<char> characters)
    {
        IOpt<string> header = new IOpt<string>.Empty();
        if (data.Value() != ' ' && data.Value() != '\t')
        {
            header = new Opt<string>(
                new string(characters.ToArray())
            );
            
            characters.Clear();
        }

        return header;
    }

    private static char LegalCharacter(IOpt<int> data, Stream stream, int position)
    {
        if ((data.Value() > 0x7f || data.Value() < 0x20) && data.Value() != '\t')
        {
            throw new HttpException(
                HttpStatusCode.BadRequest,
                string.Format(
                    "Illegal character 0x{0}2x in HTTP header line #{1}: \"{2}\"",
                    data.Value(),
                    position,
                    new TextOf(stream).AsString()
                )
            );
        }
        
        return (char)data.Value();
    }

    private static IOpt<int> Data(Stream input, IOpt<int> data, bool available)
    {
        IOpt<int> res;

        if (data.Has())
        {
            res = data;
        }
        else if (available && !input.CanRead)
        {
            res = new Opt<int>(-1);
        }
        else
        {
            res = new Opt<int>(input.ReadByte());
        }

        return res;
    }
}