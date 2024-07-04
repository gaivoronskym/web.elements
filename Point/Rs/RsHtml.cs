using System.Net;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Rs;

public sealed class RsHtml : RsWrap
{
    public RsHtml(string body)
        : this(new RsWithStatus(HttpStatusCode.OK), body)
    {
    }

    public RsHtml(IResponse origin, string body)
        : this(
            new RsWithStatus(
                new RsWithBody(
                    origin,
                    new InputOf(
                        new TextOf(body)
                    ).Stream()
                ), HttpStatusCode.OK
            )
        )
    {
    }

    public RsHtml(IResponse origin)
        : base(new RsWithType(origin, "text/html"))
    {
    }
}