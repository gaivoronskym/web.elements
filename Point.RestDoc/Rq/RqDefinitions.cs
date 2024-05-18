using System.Text.Json.Nodes;

namespace Point.RestDoc.Rq;

public sealed class RqDefinitions : IDoc
{
    private readonly IEnumerable<IDoc> _docs;

    public RqDefinitions(params IDoc[] docs)
    {
        _docs = docs;
    }

    public string Key()
    {
        return "definitions";
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