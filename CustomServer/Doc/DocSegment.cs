using Point.RestDoc;

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
            )
        );
    }
}