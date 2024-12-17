namespace Web.Elements.Rs;

public abstract class RsWrap : IResponse
{
    private readonly IResponse origin;

    public RsWrap(IResponse origin)
    {
        this.origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return origin.Head();
    }

    public Stream Body()
    {
        return origin.Body();
    }
}