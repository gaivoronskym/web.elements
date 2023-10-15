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
    private const string Header = "Authorization";
    private const string Bearer = "Bearer ";

    private readonly string _issuer;
    private readonly string _audience;

    private readonly HMAC _signature;

    public PsBearer(string issuer, string audience, string key)
        : this(issuer, audience, new HMACSHA256(new BytesOf(key).AsBytes()))
    {

    }

    public PsBearer(string issuer, string audience, HMAC signature)
    {
        _issuer = issuer;
        _audience = audience;
        _signature = signature;
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

        var token = new Split(header, Bearer).LastOrDefault(string.Empty);

        if (string.IsNullOrEmpty(token))
        {
            return new Anonymous();
        }

        IList<string> parts = new ListOf<string>(
            token.Split('.')
        );

        if(parts.Count != 3)
        {
            return new Anonymous();
        }

        byte[] jwtHeader = new BytesOf(parts[0]).AsBytes();
        byte[] jwtPayload = new BytesOf(parts[1]).AsBytes();
        byte[] jwtSign = new BytesOf(parts[2]).AsBytes();
        byte[] toCheck = jwtHeader.Concat(new BytesOf(".").AsBytes())
                .Concat(jwtPayload).ToArray();

        byte[] checkedBytes = new BytesBase64Url(_signature.ComputeHash(toCheck)).AsBytes();
        if (jwtSign.SequenceEqual(checkedBytes))
        {
            return new IdentityUser(
                    JsonNode.Parse(
                    new Base64UrlBytes(jwtPayload).AsBytes()
               )!.AsObject()
              );
        }

        return new Anonymous();

        bool ValidHeader(string item) => new And(
            new StartsWith(new TextOf(item), Header),
            new Contains(new TextOf(item), new TextOf(Bearer))
        ).Value();

    }

    public IResponse Exit(IResponse response, IIdentity identity)
    {
        return response;
    }
}

