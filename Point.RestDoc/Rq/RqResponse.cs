using System.Net;
using System.Text.Json.Nodes;

namespace Point.RestDoc.Rq;

public sealed class RqResponse : IDoc
{
    private readonly HttpStatusCode _status;
    private readonly string _description;
    private readonly IEnumerable<IDoc> _docs;
    public RqResponse(HttpStatusCode status, string description, params IDoc[] docs)
    {
        _status = status;
        _description = description;
        _docs = docs;
    }

    public string Key()
    {
        return Convert.ToUInt32(_status).ToString();
    }

    public JsonNode Content()
    {
        var json = new JsonObject { { "description", _description } };

        foreach (var doc in _docs)
        {
            json.Add(doc.Key(), doc.Content());
        }
        
        return json;
    }
}