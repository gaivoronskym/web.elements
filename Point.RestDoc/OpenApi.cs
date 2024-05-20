using System.Text.Json.Nodes;
using Yaapii.Atoms.List;

namespace Point.RestDoc;

public sealed class OpenApi : IDoc
{
    private readonly IEnumerable<IDoc> _docs;
    private readonly IEnumerable<string> _schemes;

    public OpenApi(params IDoc[] docs)
        : this(
            new ListOf<string>(
                "http",
                "https"
            ),
            docs
        )
    {

    }

    public OpenApi(IEnumerable<string> schemes, params IDoc[] docs)
    {
        _schemes = schemes;
        _docs = docs;
    }

    public string Key()
    {
        return "Data";
    }

    public JsonNode Content()
    {
        var json = new JsonObject { { "swagger", "2.0" } };
        
        foreach (var doc in _docs)
        {
            json.Add(doc.Key(), doc.Content());
        }

        return json;
    }
}