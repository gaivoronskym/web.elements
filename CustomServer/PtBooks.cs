using System.IO.Compression;
using System.Net;
using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.RestDoc;
using Point.RestDoc.Rq;
using Point.RestDoc.Types;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public sealed class PtBooks : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        return Task.FromResult<IResponse>(
            new RsBrotli(
                new RsJson(
                    new JsonArray(new JsonObject
                        {
                            { "Title", "Object thinking" }
                        }
                    )
                ),
                CompressionLevel.SmallestSize
            )
        );
    }
}