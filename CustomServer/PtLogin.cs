using System.Net;
using Point;
using Point.Authentication;
using Point.Authentication.Codec;
using Point.Extensions;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.Text;

namespace CustomServer;

public sealed class PtLogin : IPoint
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;
    private readonly string _key;

    public PtLogin(string issuer, string audience, int expiryMinutes, string key)
    {
        _issuer = issuer;
        _audience = audience;
        _expiryMinutes = expiryMinutes;
        _key = key;
    }

    public Task<IResponse> Act(IRequest req)
    {
        //new RsJwtJson(
        //            new IdentityUser("12345"),
        //            _issuer,
        //            _audience,
        //            _expiryMinutes,
        //            new HMACSHA256(new BytesOf(_key).AsBytes())
        //        )

        var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(1));

        return Task.FromResult<IResponse>(
            new RsWithCookie(
                new RsWithStatus(HttpStatusCode.OK),
                "Identity",
                new TextOf(new CcBase64(new CcPlain()).Encode(new IdentityUser("12345"))).AsString(),
                "Path=/",
                "HttpOnly",
                "Secure",
                expires.ToCookieDateFormat()
            )
        );
    }
}