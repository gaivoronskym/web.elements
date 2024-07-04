using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using MappedString = Yaapii.Atoms.Enumerable.Mapped<string, string>;
using MappedKvp = Yaapii.Atoms.Enumerable.Mapped<Yaapii.Atoms.IKvp, string>;

namespace Point.Rq;

public sealed class RqUri : IRqUri
{
    private readonly IRequest origin;
    private const string Host = "Host";
    private const string HeaderDelimiter = ": ";
    private const string RouteParamKey = "Route-";

    public RqUri(IRequest origin, IEnumerable<IKvp> map)
        : this(
            origin,
            new MappedKvp(
                (kvp) => $"{kvp.Key()}: {kvp.Value()}",
                map
            )
        )
    {
    }

    public RqUri(IRequest origin, IEnumerable<string> routes)
        : this(
            new RqWithHeaders(
                origin,
                new MappedString(
                    (literal) => $"{RouteParamKey}{literal}",
                    routes
                )
            )
        )
    {
    }

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

    public IQuerySet Route()
    {
        var list = new Filtered<string>(
            item => new StartsWith(
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