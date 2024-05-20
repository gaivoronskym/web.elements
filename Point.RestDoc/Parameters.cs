using System.Text.Json.Nodes;

namespace Point.RestDoc;

public sealed class Parameters : IDoc
{
    private readonly IEnumerable<IDoc> _docs;

    public Parameters(params IDoc[] docs)
    {
        _docs = docs;
    }

    public string Key()
    {
        return "parameters";
    }

    public JsonNode Content()
    {
        var array = new JsonArray();

        foreach (var doc in _docs)
        {
            array.Add(doc.Content());
        }

        return array;
    }
}