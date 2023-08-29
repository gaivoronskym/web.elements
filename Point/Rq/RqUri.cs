using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public class RqUri : IRqUri
{
    private readonly IRequest _origin;

    public RqUri(IRequest origin)
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

    public Uri Uri()
    {
        var uriHeader = new ItemAt<string>(
            new Filtered<string>(
                (item) => new StartsWith(new TextOf(item), "http:").Value(),
                _origin.Head()
            )
        ).Value();

        return new Uri(new Trimmed(uriHeader).AsString());
    }
}