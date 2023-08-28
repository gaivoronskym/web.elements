namespace Point;

public class BodyOf : IBody
{
    private readonly Stream _stream;

    public BodyOf(Stream stream)
    {
        _stream = stream;
    }

    public Stream Body()
    {
        return _stream;
    }
}