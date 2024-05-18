using System.Text.Json.Nodes;
using Point.RestDoc.Types;

namespace Point.RestDoc.Rq;

public sealed class RqProperty : IDoc
{
    private readonly string _name;
    private readonly IRqType _type;
    private readonly string _format;
    private readonly bool _required;
    private readonly string _example;
    private readonly IEnumerable<IDoc> _docs;
    
    public RqProperty(string name, IRqType type, bool required, params IDoc[] docs)
        : this(name, type, string.Empty, required, string.Empty, docs)
    {

    }
    
    public RqProperty(string name, IRqType type, string format, bool required, params IDoc[] docs)
        : this(name, type, format, required, string.Empty, docs)
    {

    }
    
    public RqProperty(string name, IRqType type, string format, bool required, string example, params IDoc[] docs)
    {
        _name = name;
        _type = type;
        _format = format;
        _required = required;
        _example = example;
        _docs = docs;
    }

    public string Key()
    {
        return _name;
    }

    public JsonNode Content()
    {
        try
        {
            var json = new JsonObject();

            foreach (var doc in _type.Docs())
            {
                json.Add(doc.Key(), doc.Content());
            }

            json.Add("nullable", !_required);

            if (!string.IsNullOrEmpty(_format))
            {
                json.Add("format", _format);
            }

            if (!string.IsNullOrEmpty(_example))
            {
                json.Add("example", _example);
            }

            foreach (var doc in _docs)
            {
                json.Add(doc.Key(), doc.Content());
            }

            return json;
        }
        catch (Exception ex)
        {
            return new JsonObject();
        }
    }
}