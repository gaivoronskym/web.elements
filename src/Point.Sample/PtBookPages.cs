using System.Net;
using System.Text.Json.Nodes;
using Point.Rs;

namespace Point.Sample;

public sealed class PtBookPages : IPtRegex
{
    public Task<IResponse> Act(IRqRegex req)
    {
        var bookId = req.Match().Groups["id"].Value;
        
        // return Task.FromResult<IResponse>(
        //     new RsJson(
        //         new JsonArray(
        //             new JsonObject
        //             {
        //                 { "PageNumber", "1" }
        //             },
        //             new JsonObject
        //             {
        //                 { "PageNumber", "2" }
        //             },
        //             new JsonObject
        //             {
        //                 { "PageNumber", "3" }
        //             }
        //         )
        //     )
        // );
        return WithTask(
            new RsWithHeaders(
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
                ),
                "user_id: 12345",
                "user_name: Test"
            )
        );
    }

    private Task<IResponse> WithTask(IResponse res)
    {
        return Task.FromResult(res);
    }
}