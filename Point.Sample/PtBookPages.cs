using System.Text.Json.Nodes;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Sample;

public sealed class PtBookPages : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        var bookId = new RqUri(req).Route().AsNumber("id").AsLong();

        throw new NullReferenceException("Some field is null");

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