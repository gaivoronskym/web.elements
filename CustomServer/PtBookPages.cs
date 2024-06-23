using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public sealed class PtBookPages : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        var skip = new RqUri(req).Query().AsNumber("skip").AsInt();
        var take = new RqUri(req).Query().AsNumber("take").AsInt();
        var bookId = new RqUri(req).Route().AsNumber("bookId").AsLong();

        return Task.FromResult<IResponse>(
            new RsJson(
                new JsonArray(
                    new JsonObject
                    {
                        { "PageNumber", "1" }
                    },
                    new JsonObject
                    {
                        { "PageNumber", "2" }
                    },
                    new JsonObject
                    {
                        { "PageNumber", "3" }
                    }
                )
            )
        );
    }
}