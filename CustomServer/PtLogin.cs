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
    // private readonly string _issuer;
    // private readonly string _audience;
    // private readonly int _expiryMinutes;
    // private readonly string _key;
    private readonly ICodec _codec;
    private readonly long _age;

    public PtLogin(ICodec codec, long age)
    {
        _codec = codec;
        _age = age;
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

        var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_age));

        return Task.FromResult<IResponse>(
            new RsWithCookie(
                new RsWithStatus(HttpStatusCode.OK),
                "Identity",
                new TextOf(_codec.Encode(new IdentityUser("12345"))).AsString(),
                "Path=/",
                "HttpOnly",
                "Secure",
                expires.ToCookieDateFormat()
            )
        );
    }
}