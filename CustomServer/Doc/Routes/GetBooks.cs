using System.Net;
using Point.RestDoc;

namespace CustomServer.Doc.Routes;

public sealed class GetBooks : ISegment
{
    public IDoc Doc()
    {
        return new RqGet(
            "Get books list",
            "GetBooks",
            new Docs(
                new RqTags(
                    "Books"
                ),
                new Parameters(
                    new Parameter(
                        "name",
                        "query",
                        true,
                        "string",
                        "string"
                    )
                ),
                new RqResponses(
                    new RqResponse(
                        HttpStatusCode.OK,
                        "Description"
                    ),
                    new RqResponse(
                        HttpStatusCode.NotFound,
                        "Description"
                    )
                ),
                new RqConsumes("application/json"),
                new RqProduces("application/json")
            )
        );
    }
}