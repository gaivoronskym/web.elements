using Point.Authentication.Interfaces;
using Point.Rs;
using System.Net;
using System.Text.Json.Nodes;
using Yaapii.Atoms;
using System.Security.Cryptography;
using Point.Bytes;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;
using Yaapii.Atoms.Scalar;

namespace Point.Authentication.Rs
{
    public class RsJwtJson : RsWrap
    {
        public RsJwtJson(IIdentity identity, string issuer, string audience, int expiryMinutes, HMAC signature)
        : this(
             new ScalarOf<JsonNode>(() =>
             {
                 IToken jwtHeader = new JwtHeader(signature.HashName);
                 IToken jwtPayload = new JwtPayload(
                     identity,
                     issuer,
                     audience,
                     //"iNivDmHLpUA223sqsfhqGbMRdRj1PVkH",
                     expiryMinutes
                 );

                 byte[] token = jwtHeader.Encoded()
                             .Concat(new BytesOf(".").AsBytes())
                             .Concat(jwtPayload.Encoded())
                             .ToArray();

                 byte[] sign = new BytesBase64Url(signature.ComputeHash(token)).AsBytes();

                 token = token.Concat(new BytesOf(".").AsBytes())
                              .Concat(sign)
                              .ToArray();

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
