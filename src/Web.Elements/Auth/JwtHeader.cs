using System.Text.Json.Nodes;
using Web.Elements.Bytes;
using Yaapii.Atoms.Bytes;

namespace Web.Elements.Auth;

public class JwtHeader : IToken
{
    private readonly string _algorithm;

    public const string Algorithm = "alg";
    public const string Typ = "typ";

    public JwtHeader(string algorithm)
    {
        _algorithm = algorithm;
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
        var node = new JsonObject();
        node.Add(Algorithm, _algorithm);
        node.Add(Typ, "JWT");

        return node;
    }
}