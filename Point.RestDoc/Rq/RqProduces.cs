using System.Text.Json.Nodes;

namespace Point.RestDoc.Rq;

public sealed class RqProduces : IDoc
{
    private readonly IEnumerable<string> _types;

    public RqProduces(params string[] types)
    {
        _types = types;
    }

    public string Key()
    {
        return "produces";
    }

    public JsonNode Content()
    {
        var array = new JsonArray();
        
        foreach (var type in _types)
        {
            array.Add(type);
        }

        return array;
    }
}