using System.Text.Json.Nodes;

namespace Point.RestDoc;

public abstract class DocWrap : IDoc
{
    private readonly string _key;
    private readonly JsonNode _node;
    
    protected DocWrap(string key, JsonNode node)
    {
        _key = key;
        _node = node;
    }

    public string Key()
    {
        return _key;
    }

    public JsonNode Content()
    {
        return _node;
    }
}