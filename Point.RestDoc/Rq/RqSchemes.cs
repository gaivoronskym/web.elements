using System.Text.Json.Nodes;

namespace Point.RestDoc.Rq;

public sealed class RqSchemes : IDoc
{
    private readonly IEnumerable<string> _schemes;

    public RqSchemes(params string[] schemes)
    {
        _schemes = schemes;
    }

    public string Key()
    {
        return "schemes";
    }

    public JsonNode Content()
    {
        var schemes = new JsonArray();
        
        foreach (var scheme in _schemes)
        {
            schemes.Add(scheme);
        }

        return schemes;
    }
}