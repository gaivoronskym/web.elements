using Point.Rq.Interfaces;

namespace Point.Rq;

public class RqWrap : IRequest
{
    private readonly IRequest _origin;

    public RqWrap(IRequest origin)
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