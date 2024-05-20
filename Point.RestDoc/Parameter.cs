using System.Text.Json.Nodes;

namespace Point.RestDoc;

public sealed class Parameter : IDoc
{
    private readonly string _name;
    private readonly string _in;
    private readonly bool _required;
    private readonly string _type;
    private readonly string _format;
    private readonly string _ref;

    public Parameter(string name, string @in, string rqRef)
        : this(name, @in, false, string.Empty, string.Empty, rqRef)
    {
        
    }
    
    public Parameter(string name, string @in, bool required, string type, string format)
        : this(name, @in, required, type, format, string.Empty)
    {
        
    }
    
    public Parameter(string name, string @in, bool required, string type, string format, string rqRef)
    {
        _name = name;
        _in = @in;
        _required = required;
        _type = type;
        _format = format;
        _ref = rqRef;
    }

    public string Key()
    {
        return "parameter";
    }

    public JsonNode Content()
    {
        var json = new JsonObject
        {
            { "name", _name },
            { "in", _in },
            { "required", _required },
        };

        if (!string.IsNullOrEmpty(_type))
        {
            json.Add("type", _type);
        }

        if (!string.IsNullOrEmpty(_format))
        {
            json.Add("format", _format);
        }

        if (!string.IsNullOrEmpty(_ref))
        {
            json.Add(
                "schema",
                new JsonObject
                {
                    { "$ref", _ref }
                }
            );
        }

        return json;
    }
}