using System.Text.Json.Nodes;
using Point.RestDoc.Types;
using Yaapii.Atoms.List;

namespace Point.RestDoc;

public sealed class DocOf : DocWrap
{
    public DocOf(string key, IRqType type)
        : this (key, type.Docs())
    {
        
    }
    
    public DocOf(string key, IDoc value)
        : this(key, new ListOf<IDoc>(value))
    {
        
    }
    
    public DocOf(string key, IEnumerable<IDoc> values)
        : this(key, () =>
        {
            var json = new JsonObject();
            
            foreach (var value in values)
            {
                json.Add(value.Key(), value.Content());
            }

            return json;
        })
    {
        
    }
    
    public DocOf(string key, int value)
        : this(key, () =>
        {
            var json = new JsonObject
            {
                { key, value }
            };
            var jsonValue = json[key]!;

            json.Remove(key);
            
            return jsonValue;
        })
    {
        
    }
    
    public DocOf(string key, bool value)
        : this(key, () =>
        {
            var json = new JsonObject
            {
                { key, value }
            };
            var jsonValue = json[key]!;

            json.Remove(key);
            
            return jsonValue;
        })
    {
        
    }
    
    public DocOf(string key, string value)
        : this(key, () =>
        {
            var json = new JsonObject
            {
                { key, value }
            };
            var jsonValue = json[key]!;

            json.Remove(key);
            
            return jsonValue;
        })
    {
        
    }
    
    public DocOf(string key, JsonNode json)
        : this(key, () => json)
    {
        
    }
    
    public DocOf(string key, Func<JsonNode> func) : base(key, func())
    {
    }
}