using System.IO.Compression;
using System.Text.Json.Nodes;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Sample;

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