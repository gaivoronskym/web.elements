using System.Net;
using System.Text.Json.Nodes;
using Yaapii.Atoms.Text;

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
                new Formatted("{0}: {1}", "Content-Type", "application/json").AsString()
            )
        )
    {
    }
}