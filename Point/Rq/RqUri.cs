using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public sealed class RqUri : IRqUri
{
    private readonly IRequest _origin;
    private const string Host = "Host";
    private const string HeaderDelimiter = ": ";
    private const string RouteParamKey = "Route56321-";
    
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
        var host = new ItemAt<string>(
            new Filtered<string>(
                (item) => new StartsWith(new TextOf(item), Host).Value(),
                _origin.Head()
            )
        ).Value();

        var firstHead = new ItemAt<string>(
            _origin.Head()
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

    public IQuerySet Route()
    {
        var list = new Filtered<string>(
            (item) => new StartsWith(
                new TextOf(item),
                RouteParamKey
            ).Value(),
            Head()
        );
        
        var map = new QuerySet();
        
        foreach (var line in list)
        {
            var splitParam = new Split(line, ": ");
            var key = splitParam.First().Remove(0, RouteParamKey.Length);
            var value = splitParam.Last();
            map.Add(key, value);
        }

        return map;
    }

    public IQuerySet Query()
    {
        var query = Uri().Query;
        
        if (string.IsNullOrEmpty(query))
        {
            return new QuerySet();
        }

        query = query.TrimStart('?');
        
        var list = new ListOf<string>(
            new Split(query, "&")
        );

        var map = new QuerySet();
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