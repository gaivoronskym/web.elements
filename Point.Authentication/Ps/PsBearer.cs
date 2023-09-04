using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Point.Authentication.Interfaces;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Contains = Yaapii.Atoms.Text.Contains;

namespace Point.Authentication.Ps;

public class PsBearer : IPass
{
    private const string Header = "Authorization";
    private const string Bearer = "Bearer ";

    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;

    public PsBearer(string issuer, string audience, string key)
    {
        _issuer = issuer;
        _audience = audience;
        _key = key;
    }

    public IIdentity Enter(IRequest req)
    {

        try
        {
            var header = new ItemAt<string>(
                new Filtered<string>(
                    ValidHeader,
                    req.Head()
                ),
                string.Empty
            ).Value();

            if (string.IsNullOrEmpty(header))
            {
                return new Anonymous();
            }

            var token = new Split(header, Bearer).LastOrDefault(string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return new Anonymous();
            }

            //parsing token...

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _key
                )
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var claims = jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = _issuer,
                ValidateIssuer = true,
                ValidAudience = _audience,
                ValidateAudience = true,
                IssuerSigningKey = signinKey,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true
            }, out var securityToken);

            var jwtToken = (JwtSecurityToken)securityToken;

            IDictionary<string, string> data = new Dictionary<string, string>();
            var nameId = claims.FindFirst(ClaimTypes.NameIdentifier);

            foreach (var claim in claims.Claims)
            {
                data.Add(claim.Type, claim.Value);
            }

            return new IdentityUser(nameId.Value, data);
        }
        catch (Exception ex)
        {
            return new Anonymous();
        }

        bool ValidHeader(string item) => new And(
            new StartsWith(new TextOf(item), Header),
            new Contains(new TextOf(item), new TextOf(Bearer))
        ).Value();
    }
}