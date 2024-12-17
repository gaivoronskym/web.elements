using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Web.Elements.Auth;
using Web.Elements.Bytes;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Web.Elements.Ps;

public sealed class PsToken : IPass
{
    private readonly HMAC signature;
    private readonly string header;
    private readonly long seconds;

    public PsToken(string key)
        : this(new HMACSHA256(new BytesOf(key).AsBytes()), 3600)
    {
    }
    
    public PsToken(string key, long seconds)
        : this(new HMACSHA256(new BytesOf(key).AsBytes()), seconds)
    {
    }

    public PsToken(HMAC signature, long seconds)
    {
        this.signature = signature;
        this.header = "Authorization";
        this.seconds = seconds;
    }

    public IOptinal<IIdentity> Enter(IRequest req)
    {
        IOptinal<IIdentity> user = new Optinal<IIdentity>(new Anonymous());

        var head = new ItemAt<IText>(
            new Filtered<IText>(
                text => new StartsWith(
                    new Trimmed(
                        text
                    ),
                    new TextOf("Bearer")
                ).Value(),
                new Mapped<string, IText>(
                    text => new TextOf(text),
                    new IRqHeaders.Base(req).Header(this.header)
                )
            ),
            new TextOf(string.Empty)
        ).Value();

        if (!string.IsNullOrEmpty(head.AsString()))
        {
            var jwt = new Trimmed(head.AsString().Split(" ", 2)[1]).AsString();
            var parts = jwt.Split(".");
            
            var jwtHeader = new BytesOf(parts[0]).AsBytes();
            var jwtPayload = new BytesOf(parts[1]).AsBytes();
            var jwtSign = new BytesOf(parts[2]).AsBytes();
            var toCheck = jwtHeader.Concat(new BytesOf(".").AsBytes()).Concat(jwtPayload).ToArray();
            
            var checkedBytes = new BytesBase64Url(signature.ComputeHash(toCheck)).AsBytes();
            if (jwtSign.SequenceEqual(checkedBytes))
            {
                user = new Optinal<IIdentity>(
                    new IdentityUser(
                        JsonNode.Parse(
                            new Base64UrlBytes(jwtPayload).AsBytes()
                        )!.AsObject()
                    )
                );
            }
        }

        return user;

    }

    public IResponse Exit(IResponse response, IIdentity identity)
    {
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

        var json = JsonNode.Parse(response.Body());
        var jwt = Encoding.Default.GetString(token);

        return new RsJson(
            new JsonObject
            {
                { "response", json },
                { "jwt", jwt },
            }
        );
    }
}

