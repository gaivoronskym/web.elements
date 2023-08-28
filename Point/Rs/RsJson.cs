using System.Net;
using System.Text.Json.Nodes;

namespace Point.Rs;

public class RsJson : RsWrap
{
    public RsJson(JsonNode json)
        : this(
            new RsWithBody(
                json.ToJsonString()
            )
        )
    {
    }
    
    public RsJson(IResponse origin)
        : base(
            new RsWithHeader(
                new RsWithStatus(
                    origin,
                    HttpStatusCode.OK
                ),
                "Content-Type", "application/json"
            )
        )
    {
    }
}