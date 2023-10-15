using Point.Authentication.Interfaces;
using Point.Bytes;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using Yaapii.Atoms.Bytes;

namespace Point.Authentication;

public sealed class JwtPayload : IToken
{
    private readonly IIdentity _identity;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;

    private const string Issuer = "iss";
    private const string Audience = "aud";
    private const string Expiration = "exp";

    public JwtPayload(IIdentity identity,
        string issuer,
        string audience,
        int expiryMinutes
       )
    {
        _identity = identity;
        _issuer = issuer;
        _audience = audience;
        _expiryMinutes = expiryMinutes;
    }

    public byte[] Encoded()
    {
        return new BytesBase64Url(
                    new BytesOf(
                        Json().ToJsonString(),
                        Encoding.UTF8
                     )
               ).AsBytes();
    }

    public JsonObject Json()
    {
        var expiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_expiryMinutes));

        JsonObject node = new JsonObject();
        node.Add(Issuer, _issuer);
        node.Add(Audience, _audience);
        node.Add(Expiration, expiration.Ticks);
        node.Add(ClaimTypes.NameIdentifier, _identity.Identifier());

        foreach (var property in _identity.Properties())
        {
            node.Add(property.Key, property.Value);
        }

        return node;
    }
}