using System.Text.Json.Nodes;
using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace Point.Sample;

public sealed class PtBookAuthors : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        var query = new IRqHref.Base(req).Href().Param("author");

        return Task.FromResult<IResponse>(new RsJson(
                new JsonObject
                {
                    { "Author", "Author Name" }
                }
            )
        );
    }
}