using Point.Authentication.Interfaces;
using Point.Rs;
using System.Net;
using System.Text.Json.Nodes;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;
using Yaapii.Atoms.Scalar;

namespace Point.Authentication.Rs
{
    public class RsJwtJson : RsWrap
    {
        public RsJwtJson(IIdentity identity, ITokenFactory tokenFactory)
            : this(
                new Live<JsonNode>(() =>
                {
                    var token = tokenFactory.Bytes(identity);
                    var str = new TextOf(token).AsString();

                    return new JsonObject
                    {
                        { "jwt", str }
                    };
                })
            )
        {
        }

        private RsJwtJson(IScalar<JsonNode> node)
            : this(node.Value())
        {
        }

        private RsJwtJson(JsonNode json)
            : this(
                new RsWithBody(
                    json.ToJsonString()
                )
            )
        {
        }

        private RsJwtJson(IResponse origin)
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
}
