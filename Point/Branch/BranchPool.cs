using Point.Rq.Interfaces;

namespace Point.Branch;

public sealed class BranchPool : IBranch
{
    private readonly IList<IBranch> _branches;

    public BranchPool(IList<IBranch> branches)
    {
        _branches = branches;
    }

    public BranchPool(params IBranch[] branches)
    {
        _branches = branches;
    }
    
    public IResponse? Route(IRequest req)
    {
        foreach (var branch in _branches)
        {
            IResponse? response = branch.Route(req);

            if (response is not null)
            {
                return response;
            }
        }

        return default;
    }
}