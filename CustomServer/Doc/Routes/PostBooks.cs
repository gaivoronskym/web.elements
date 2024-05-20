using System.Net;
using Point.RestDoc;
using Point.RestDoc.Rq;

namespace CustomServer.Doc.Routes;

public sealed class PostBooks : ISegment
{
    public IDoc Doc()
    {
        return new RqPost(
            "Add book",
            "PostBooks",
            new Docs(
                new RqTags(
                    "Books"
                ),
                new Parameters(
                    new Parameter(
                        "book",
                        "body",
                        "#/definitions/book"
                    )
                ),
                new RqResponses(
                    new RqResponse(
                        HttpStatusCode.OK,
                        "Success"
                    )
                ),
                new RqConsumes("application/json"),
                new RqProduces("application/json")
            )
        );
    }
}