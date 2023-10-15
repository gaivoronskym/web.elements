using System.Security.Cryptography;
using Point;
using Point.Authentication;
using Point.Authentication.Rs;
using Point.Pt;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Bytes;

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
        return Task.FromResult<IResponse>(
            new RsJwtJson(
                    new IdentityUser("12345"),
                    _issuer,
                    _audience,
                    _expiryMinutes,
                    new HMACSHA256(new BytesOf(_key).AsBytes())
                )
        );
    }
}