using System.Net;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Rs;

public sealed class RsWithBody : RsWrap
{
    public RsWithBody(string body)
        : base(
            new RsWithHeader(
                new ResponseOf(
                    new RsWithStatus(HttpStatusCode.OK)
                        .Head,
                    new InputOf(
                        new TextOf(
                            body
                        )
                    ).Stream
                ),
                "Content-Length", body.Length.ToString()
            )
        )
    {
    }

    public RsWithBody(IResponse response, string body)
        : this(response, new BytesOf(body).AsBytes())
    {
    }

    public RsWithBody(IResponse origin, byte[] body)
        : this(origin, new MemoryStream(body))
    {
    }

    public RsWithBody(Stream body)
        : base(
                new RsWithHeader(
                    new ResponseOf(
                        new RsWithStatus(HttpStatusCode.OK)
                            .Head,
                        () => body
                    ),
                    "Content-Length", body.Length.ToString()
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
