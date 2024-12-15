using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Point.Auth;
using Point.Bytes;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Sample;

public sealed class PtLogin : IPoint
{
    private readonly HMAC signature;
    private readonly long seconds;
    
    public PtLogin(long seconds, string key)
    {
        this.signature = new HMACSHA256(new BytesOf(key).AsBytes());
        this.seconds = seconds;
    }

    public Task<IResponse> Act(IRequest req)
    {
        var body = new TextOf(
            new InputOf(
                req.Body()
            )
        ).AsString();
        
        var identity = new IdentityUser("user");
        IToken jwtHeader = new JwtHeader(signature.HashName);
        IToken jwtPayload = new JwtPayload(
            identity,
            this.seconds
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

        var jwt = Encoding.Default.GetString(token);
        
        var json = new JsonObject
        {
            {"jwt", jwt}
        };
        return this.FromTask(new RsJson(json));
    }

    private Task<IResponse> FromTask(IResponse res)
    {
        return Task.FromResult(res);
    }
}