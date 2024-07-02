using Point.Authentication.Interfaces;
using Point.Bytes;
using Point.Rq.Interfaces;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Contains = Yaapii.Atoms.Text.Contains;

namespace Point.Authentication.Ps;

public sealed class PsBearer : IPass
{
    private readonly HMAC signature;

    public PsBearer(string key)
        : this(new HMACSHA256(new BytesOf(key).AsBytes()))
    {
    }

    public PsBearer(HMAC signature)
    {
        this.signature = signature;
    }

    public IIdentity Enter(IRequest req)
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

        var token = new Split(header, "Bearer ").LastOrDefault(string.Empty);

        if (string.IsNullOrEmpty(token))
        {
            return new Anonymous();
        }

        var parts = new ListOf<string>(
            token.Split('.')
        );

        if(parts.Count != 3)
        {
            return new Anonymous();
        }

        var jwtHeader = new BytesOf(parts[0]).AsBytes();
        var jwtPayload = new BytesOf(parts[1]).AsBytes();
        var jwtSign = new BytesOf(parts[2]).AsBytes();
        var toCheck = jwtHeader.Concat(new BytesOf(".").AsBytes())
                .Concat(jwtPayload).ToArray();

        var checkedBytes = new BytesBase64Url(signature.ComputeHash(toCheck)).AsBytes();
        if (jwtSign.SequenceEqual(checkedBytes))
        {
            return new IdentityUser(
                JsonNode.Parse(
                    new Base64UrlBytes(jwtPayload).AsBytes()
                )!.AsObject()
            );
        }

        return new Anonymous();

    }

    public IResponse Exit(IResponse response, IIdentity identity)
    {
        return response;
    }
    
    private bool ValidHeader(string item) => new And(
        new StartsWith(new TextOf(item), "Authorization"),
        new Contains(new TextOf(item), new TextOf("Bearer "))
    ).Value();
}

