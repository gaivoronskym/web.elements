using Point;
using Point.Authentication;
using Point.Authentication.Interfaces;
using Point.Authentication.Rs;
using Point.Pt;
using Point.Rq.Interfaces;

namespace CustomServer;

public sealed class PtLogin : IPoint
{
    private readonly ITokenFactory tokenFactory;

    public PtLogin(ITokenFactory tokenFactory)
    {
        this.tokenFactory = tokenFactory;
    }

    public Task<IResponse> Act(IRequest req)
    {
        var res = new RsJwtJson(
            new IdentityUser("12345"),
            tokenFactory
        );

        return Task.FromResult<IResponse>(res);

        // var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_age));

        // return Task.FromResult<IResponse>(
        //     new RsWithCookie(
        //         new RsWithStatus(HttpStatusCode.OK),
        //         "Identity",
        //         new TextOf(_codec.Encode(new IdentityUser("12345"))).AsString(),
        //         "Path=/",
        //         "HttpOnly",
        //         "Secure",
        //         expires.ToCookieDateFormat()
        //     )
        // );
    }
}