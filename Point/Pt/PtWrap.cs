using Point.Rq.Interfaces;

namespace Point.Pt;

public abstract class PtWrap : IPoint
{
    protected readonly IPoint Origin;

    public PtWrap(IPoint origin)
    {
        Origin = origin;
    }

    public abstract IResponse Act(IRequest req);
}