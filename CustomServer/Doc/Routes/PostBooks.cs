using System.Net;
using Point.RestDoc;

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
                        "title",
                        "body",
                        "#/definitions/Book"
                    )
                ),
                new RqResponses(
                    new RqResponse(
                        HttpStatusCode.OK,
                        "Description"
                    )
                ),
                new RqConsumes("application/json"),
                new RqProduces("application/json")
            )
        );
    }
}