using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Point.Authentication.Interfaces;

namespace Point.Authentication;

public class JwtToken : IToken
{
    private readonly IIdentity _identity;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;
    private readonly int _expiryMinutes;

    public JwtToken(IIdentity identity, string issuer, string audience, string key, int expiryMinutes)
    {
        _identity = identity;
        _issuer = issuer;
        _audience = audience;
        _key = key;
        _expiryMinutes = expiryMinutes;
    }

    public string AsString()
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
    }
}