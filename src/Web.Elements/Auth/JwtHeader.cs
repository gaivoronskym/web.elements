using System.Text.Json.Nodes;
using Web.Elements.Bytes;
using Yaapii.Atoms.Bytes;

namespace Web.Elements.Auth;

public class JwtHeader : IToken
{
    private readonly string alg;

    private const string algorithm = "alg";
    private const string typ = "typ";

    public JwtHeader(string alg)
    {
        this.alg = alg;
    }

    public byte[] Encoded()
    {
        return new BytesBase64Url(
            new BytesOf(
                Json().ToJsonString()
            )
        ).AsBytes();
    }

    public JsonObject Json()
    {
        var node = new JsonObject
        {
          { algorithm, alg },
          { typ, "JWT" }
        };

        return node;
    }
}