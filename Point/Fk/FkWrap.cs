using Point.Rq.Interfaces;

namespace Point.Fk;

public abstract class FkWrap : IFork
{
    private readonly IFork _origin;

    public FkWrap(IFork origin)
    {
        _origin = origin;
    }

    public virtual Task<IResponse?> Route(IRequest req)
    {
        return _origin.Route(req);
    }
}