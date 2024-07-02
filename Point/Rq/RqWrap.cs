using Point.Rq.Interfaces;

namespace Point.Rq;

public abstract class RqWrap : IRequest
{
    private readonly IRequest origin;

    public RqWrap(IRequest origin)
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