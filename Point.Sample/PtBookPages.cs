using System.Text.Json.Nodes;
using Point.Rs;

namespace Point.Sample;

public sealed class PtBookPages : IPtRegex
{
    public Task<IResponse> Act(IRqRegex req)
    {
        var bookId = req.Match().Groups["id"].Value;
        
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