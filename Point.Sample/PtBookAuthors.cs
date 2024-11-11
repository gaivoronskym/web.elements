using System.Text.Json.Nodes;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Sample;

public sealed class PtBookAuthors : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        var paramList = new RqUri(req).Route();
        var query = new RqUri(req).Query();

        return Task.FromResult<IResponse>(new RsJson(
                new JsonObject
                {
                    { "Author", "Author Name" }
                }
            )
        );
    }
}