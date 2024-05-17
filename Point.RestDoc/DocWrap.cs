using System.Text.Json.Nodes;

namespace Point.RestDoc;

public abstract class DocWrap : IDoc
{
    private readonly string _key;
    private readonly Func<JsonNode> _func;

    protected DocWrap(string key, Func<JsonNode> func)
    {
        _key = key;
        _func = func;
    }
    
    protected DocWrap(string key, JsonNode node)
        : this(key, () => node)
    {
    }

    public string Key()
    {
        return _key;
    }

    public JsonNode Content()
    {
        return _func();
    }
}