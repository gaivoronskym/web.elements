using System.Text.Json.Nodes;

namespace Point.RestDoc;

public sealed class Route : IDoc
{
    private readonly string _route;
    private readonly IEnumerable<IDoc> _docs;

    public Route(string route, params IDoc[] docs)
    {
        _route = route;
        _docs = docs;
    }

    public string Key()
    {
        return _route;
    }

    public JsonNode Content()
    {
        var json = new JsonObject();

        foreach (var doc in _docs)
        {
            json.Add(doc.Key(), doc.Content());
        }

        return json;
    }
}