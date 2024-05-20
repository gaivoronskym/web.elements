using System.Text.Json.Nodes;

namespace Point.RestDoc;

public sealed class Info : IDoc
{
    private readonly string _title;
    private readonly string _description;
    private readonly string _version;

    public Info(string title, string version)
        : this(title, string.Empty, version)
    {
        
    }
    
    public Info(string title, string description, string version)
    {
        _title = title;
        _description = description;
        _version = version;
    }

    public string Key()
    {
        return "info";
    }

    public JsonNode Content()
    {
        return new JsonObject
        {
            { "title", _title },
            { "description", _description },
            { "version", _version }
        };
    }
}