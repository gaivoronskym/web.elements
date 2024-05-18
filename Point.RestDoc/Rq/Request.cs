using System.Text.Json.Nodes;

namespace Point.RestDoc.Rq;

public abstract class Request : IDoc
{
    private readonly string _method;
    private readonly string _summary;
    private readonly string _operationId;
    private readonly IEnumerable<IDoc> _docs;

    protected Request(string method, string summary, string operationId, IEnumerable<IDoc> docs)
    {
        _method = method;
        _summary = summary;
        _operationId = operationId;
        _docs = docs;
    }
    
    public string Key()
    {
        return _method;
    }

    public JsonNode Content()
    {
        var get = new JsonObject
        {
            { "summary", _summary },
        };

        if (!string.IsNullOrEmpty(_operationId))
        {
            get.Add("operationId", _operationId);
        }
        
        foreach (var doc in _docs)
        {
            get.Add(doc.Key(), doc.Content());
        }

        return get;
    }
}