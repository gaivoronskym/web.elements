using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;
using MappedString = Yaapii.Atoms.Enumerable.Mapped<string, string>;
using MappedKvp = Yaapii.Atoms.Enumerable.Mapped<Yaapii.Atoms.IKvp, string>;

namespace Point.Rq;

public sealed class RqUri : IRqUri
{
    private readonly IRequest origin;
    private const string Host = "Host";
    private const string HeaderDelimiter = ": ";

    public RqUri(IRequest origin)
    {
        this.origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return origin.Head();
    }

    public Stream Body()
    {
        return origin.Body();
    }

    public Uri Uri()
    {
        var host = new ItemAt<string>(
            new Filtered<string>(
                item => new StartsWith(new TextOf(item), Host).Value(),
                origin.Head()
            )
        ).Value();

        var firstHead = new ItemAt<string>(
            origin.Head()
        ).Value();

        using var splitHeader = new Split(
            firstHead,
            " "
        ).GetEnumerator();
        splitHeader.MoveNext();
        splitHeader.MoveNext();
        
        var path = splitHeader.Current;
        
        var splitHost = new Split(host, HeaderDelimiter);
        
        return new Uri(new Trimmed($"http://{splitHost.Last()}{path}").AsString());
    }

    public IDictionary<string, string> Query()
    {
        var query = Uri().Query;
        
        if (string.IsNullOrEmpty(query))
        {
            return new Dictionary<string, string>();
        }

        query = query.TrimStart('?');
        
        var list = new ListOf<string>(
            new Split(query, "&")
        );

        var map = new Dictionary<string, string>();
        foreach (var queryParam in list)
        {
            var splittedParam = new Split(
                queryParam,
                "="
            );
            var key = splittedParam.First();
            var value = splittedParam.Last();
            map.Add(key, value);
        }

        return map;
    }
}