using Point.Rq.Interfaces;

namespace Point.Fk;

public abstract class FkWrap : IFork
{
    private readonly IFork origin;

    public FkWrap(IFork origin)
    {
        this.origin = origin;
    }

    public virtual Task<IOpt<IResponse>> Route(IRequest req)
    {
        return origin.Route(req);
    }
}