using System.Text.Json.Nodes;
using Yaapii.Atoms.Map;

namespace Point.RestDoc;

public sealed class DocsMap : MapEnvelope<JsonNode>
{
    public DocsMap(IEnumerable<IDoc> docs)
        : this(() => docs.ToDictionary(x => x.Key(), x => x.Content()), true)
    {
        
    }
    
    public DocsMap(Func<IDictionary<string, JsonNode>> origin, bool live) : base(origin, live)
    {
    }
}