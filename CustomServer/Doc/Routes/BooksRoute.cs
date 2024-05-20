using Point.RestDoc;

namespace CustomServer.Doc.Routes;

public sealed class BooksRoute : ISegment
{
    public IDoc Doc()
    {
        return new Route(
            "/books",
            new PostBooks().Doc(),
            new GetBooks().Doc()
        );
    }
}