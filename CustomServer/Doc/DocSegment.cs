using Point.RestDoc;
using Point.RestDoc.Rq;
using Point.RestDoc.Types;

namespace CustomServer.Doc;

public class DocSegment : ISegment
{
    private readonly IEnumerable<ISegment> _segments;

    public DocSegment(params ISegment[] segments)
    {
        _segments = segments;
    }

    public IDoc Doc()
    {
        return new OpenApi(
            new Info(
                "My API",
                "My API description",
                "1.0.0"
            ),
            new RqSchemes("http", "https"),
            new RqConsumes("application/json"),
            new RqProduces("application/json"),
            new Paths(
                _segments.Select(s => s.Doc()).ToArray()
            ),
            new RqDefinitions(
                new RqProperty(
                    "book",
                    new RqObject(),
                    true,
                    new RqProperties(
                        new RqProperty(
                            "title",
                            new RqString(),
                            true
                        ),
                        new RqProperty(
                            "description",
                            new RqString(),
                            true
                        ),
                        new RqProperty(
                            "authors",
                            new RqArrayOf(
                                new RqString()
                            ),
                            false
                        )
                    )
                )
            )
        );
    }
}