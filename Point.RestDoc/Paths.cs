using System.Text.Json.Nodes;

namespace Point.RestDoc;

public sealed class Paths : IDoc
{
    private readonly IEnumerable<IDoc> _docs;

    public Paths(params IDoc[] docs)
    {
        _docs = docs;
    }

    public string Key()
    {
        return "paths";
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