using System.Net;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Rs;

public class RsWithBody : RsWrap
{
    public RsWithBody(string body)
        : base(
            new ResponseOf(
                new RsWithStatus(HttpStatusCode.OK)
                    .Head(),
                () => new InputOf(
                    new TextOf(
                        body
                    )
                ).Stream()
            )
        )
    {
    }

    public RsWithBody(IResponse origin, Stream body)
        : base(
            new ResponseOf(
                new RsWithHeader(
                        origin,
                        "Content-Length",
                        body.Length.ToString()
                    )
                    .Head(),
                () => body
            )
        )
    {
    }
}
