using Point.Auth;
using Point.Codec;
using Point.Extensions;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Contains = Yaapii.Atoms.Text.Contains;

namespace Point.Ps;

public class PsCookie : IPass
{
    private readonly ICodec codec;
    private readonly string cookie;
    //in minutes
    private readonly long age;

    public PsCookie(ICodec codec, string cookie, long age)
    {
        this.codec = codec;
        this.cookie = cookie;
        this.age = age;
    }

    public IOptinal<IIdentity> Enter(IRequest req)
    {
        var cookie = new RqCookies(req).Cookie(this.cookie);
        if (!string.IsNullOrEmpty(cookie))
        {
            return new Optinal<IIdentity>(
                codec.Decode(
                    new BytesOf(
                        cookie
                    ).AsBytes()
                )
            );
        }

        return new Optinal<IIdentity>(new Anonymous());
    }

    public IResponse Exit(IResponse response, IIdentity identity)
    {
        var cookieExists = new Contains<string>(
            response.Head(),
            item => new Contains(item, "Cookie").Value()
        );

        if (cookieExists.Value())
        {
            return response;
        }
        
        var text = string.Empty;
        if (identity is not Anonymous)
        {
            text = new TextOf(codec.Encode(identity)).AsString();
        }
        
        var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(age));

        return new RsWithCookie(
            response,
            cookie,
            text,
            "Path=/",
            "HttpOnly",
            expires.ToCookieDateFormat()
        );
    }
}