namespace Point;

public sealed class BodyOf : IBody
{
    private readonly Stream stream;

    public BodyOf(Stream stream)
    {
        this.stream = stream;
    }

    public Stream Body()
    {
        return stream;
    }
}