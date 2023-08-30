using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public class RqHeaders : IRqHeaders
{
    private readonly IRequest _origin;

    public RqHeaders(IRequest origin)
    {
        _origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return _origin.Head();
    }

    public Stream Body()
    {
        return _origin.Body();
    }

    public IDictionary<string, string> Headers()
    {
        var heads = new Distinct<string>(
            new Filtered<string>(
                (item) => new Not(
                    new StartsWith(
                        new TextOf(item),
                        "HTTP/")
                ).Value(),
                Head()
            )
        );

        var map = new Dictionary<string, string>();
        
        foreach (var head in heads)
        {
            var splittedHead = new Split(head, ":");

            if (splittedHead.Count() != 2)
            {
                continue;
            }

            var header = new Trimmed(splittedHead.First()).AsString();
            var value = new Trimmed(splittedHead.Last()).AsString();
            
            map.Add(header, value);
        }

        return map;
    }
}