using System.Net;
using System.Text.Json.Nodes;

namespace Point.RestDoc;

public sealed class RqResponse : IDoc
{
    private readonly HttpStatusCode _status;
    private readonly string _description;

    public RqResponse(HttpStatusCode status, string description)
    {
        _status = status;
        _description = description;
    }

    public string Key()
    {
        return Convert.ToUInt32(_status).ToString();
    }

    public JsonNode Content()
    {
        var json = new JsonObject { { "description", _description } };

        return json;
    }
}