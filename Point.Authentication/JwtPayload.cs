using Point.Authentication.Interfaces;
using System.Text.Json.Nodes;
using Yaapii.Atoms.Bytes;

namespace Point.Authentication;

public sealed class JwtPayload : IToken
{
    private readonly IIdentity _identity;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;
    private readonly int _expiryMinutes;

    private const string Issuer = "iss";
    private const string Audience = "aud";
    private const string Expiration = "exp";

    public JwtPayload(IIdentity identity,
        string issuer,
        string audience,
        string key,
        int expiryMinutes
       )
    {
        _identity = identity;
        _issuer = issuer;
        _audience = audience;
        _key = key;
        _expiryMinutes = expiryMinutes;
    }

    /*public string AsString()
    {
        var userid = _identity.Identifier();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userid),
        };

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var expirationDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_expiryMinutes));

        var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _key
            )
        );

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: expirationDate,
            signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256));

        return jwtTokenHandler.WriteToken(token);
    }*/

    public byte[] Encoded()
    {
        return new BytesBase64(
                    new BytesOf(
                        Json().ToJsonString()
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

        foreach (var property in _identity.Properties())
        {
            node.Add(property.Key, property.Value);
        }

        return node;
    }
}