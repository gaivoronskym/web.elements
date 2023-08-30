using System.Net;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Map;
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

    public IDictionary<string, object> RouteParams()
    {
        var pathParams = new ItemAt<string>(
            new Filtered<string>(
                (item) => new StartsWith(
                    new TextOf(item),
                    "path:"
                ).Value(),
                Head()
            ), new HttpRequestException("Bad Request", null, HttpStatusCode.BadRequest)
        ).Value();

        if (string.IsNullOrEmpty(pathParams))
        {
            return new MapOf<object>();
        }

        var keys = new ListOf<string>(
            new Split(
                new Split(
                    pathParams,
                    ":"
                ).Last(),
                ","
            )
        );

        var segments = new ListOf<string>(
            new Split(Uri().LocalPath, "/")
        );

        var map = new Dictionary<string, object>();
        
        foreach (var key in keys)
        {
            var splittedKey = new Split(key, ";");
            var name = splittedKey.First();
            var index = int.Parse(splittedKey.Last());
            
            map.Add(name, segments[index]);
        }

        return map;
    }

    public IDictionary<string, object> Query()
    {
        var query = Uri().Query;

        if (string.IsNullOrEmpty(query))
        {
            return new MapOf<object>();
        }

        query = query.TrimStart('?');
        
        var list = new ListOf<string>(
            new Split(query, "&")
        );

        var map = new Dictionary<string, object>();
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