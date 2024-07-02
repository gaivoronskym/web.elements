using System.Security.Cryptography;
using Point.Authentication.Interfaces;
using Point.Bytes;
using Yaapii.Atoms.Bytes;

namespace Point.Authentication;

public sealed class TokenFactory : ITokenFactory
{
    private readonly string issuer;
    private readonly string audience;
    private readonly int expiryMinutes;
    private readonly HMAC signature;
    
    public TokenFactory(string issuer, string audience, int expiryMinutes, HMAC signature)
    {
        this.issuer = issuer;
        this.audience = audience;
        this.expiryMinutes = expiryMinutes;
        this.signature = signature;
    }

    public byte[] Bytes(IIdentity identity)
    {
        IToken jwtHeader = new JwtHeader(signature.HashName);
        IToken jwtPayload = new JwtPayload(
            identity,
            issuer,
            audience,
            expiryMinutes
        );
        
        var token = jwtHeader.Encoded()
            .Concat(new BytesOf(".").AsBytes())
            .Concat(jwtPayload.Encoded())
            .ToArray();

        var sign = new BytesBase64Url(
            signature.ComputeHash(
                token
            )
        ).AsBytes();

        token = token.Concat(new BytesOf(".").AsBytes())
            .Concat(sign)
            .ToArray();

        return token;
    }
}