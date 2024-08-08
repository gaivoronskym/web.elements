using Point.Authentication.Codec;
using Point.Authentication.Interfaces;
using Point.Authentication.Rq;
using Point.Extensions;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Contains = Yaapii.Atoms.Text.Contains;

namespace Point.Authentication.Ps;

public class PsCookie : IPass
{
    private readonly ICodec _codec;
    private readonly string _cookie;
    //in minutes
    private readonly long _age;

    public PsCookie(ICodec codec, string cookie, long age)
    {
        _codec = codec;
        _cookie = cookie;
        _age = age;
    }

    public IOpt<IIdentity> Enter(IRequest req)
    {
        var cookie = new RqCookies(req).Cookie(_cookie);
        if (!cookie.IsEmpty())
        {
            return new Opt<IIdentity>(
                _codec.Decode(
                    new BytesOf(
                        cookie
                    ).AsBytes()
                )
            );
        }

        return new Opt<IIdentity>(new Anonymous());
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
            text = new TextOf(_codec.Encode(identity)).AsString();
        }
        
        var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_age));

        return new RsWithCookie(
            response,
            _cookie,
            text,
            "Path=/",
            "HttpOnly",
            expires.ToCookieDateFormat()
        );
    }
}