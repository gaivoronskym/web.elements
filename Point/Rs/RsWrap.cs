namespace Point.Rs;

public class RsWrap : IResponse
{
    private readonly IResponse _origin;

    public RsWrap(IResponse origin)
    {
        _origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return _origin.Head();
    }

    public Stream Body()
    {
        return _origin.Body();
    }
}