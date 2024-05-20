using System.Net;
using Point.RestDoc;
using Point.RestDoc.Rq;
using Point.RestDoc.Types;

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
                        "Success",
                        new DocOf(
                            "schema",
                            new RqArrayOf(
                                new RqReference(
                                    "#/definitions/book"
                                )
                            )
                        )
                    ),
                    new RqResponse(
                        HttpStatusCode.NotFound,
                        "Not found"
                    )
                ),
                new RqConsumes("application/json"),
                new RqProduces("application/json")
            )
        );
    }
}