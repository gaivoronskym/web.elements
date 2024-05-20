using System.Text.Json.Nodes;

namespace Point.RestDoc;

public interface IDoc
{
    string Key();
    
    JsonNode Content();
}