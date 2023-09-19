using Point.Rq.Interfaces;

namespace Point.Branch;

public abstract class BranchWrap : IBranch
{
    private readonly IBranch _origin;

    public BranchWrap(IBranch origin)
    {
        _origin = origin;
    }

    public virtual IResponse? Route(IRequest req)
    {
        return _origin.Route(req);
    }
}