using System.Net;
using System.Text.Json.Nodes;
using Yaapii.Atoms.Text;

namespace Point.Rs;

public sealed class RsJson : RsWrap
{
    public RsJson(string json)
        : this(
            new RsWithBody(
                json
            )
        )
    {

    }

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
            new RsWithType(
                new RsWithStatus(
                    origin,
                    HttpStatusCode.OK
                ),
                "application/json"
            )
        )
    {
    }
}