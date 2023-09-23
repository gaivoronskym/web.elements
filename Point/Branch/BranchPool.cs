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
    
    public async Task<IResponse?> Route(IRequest req)
    {
        foreach (var branch in _branches)
        {
            IResponse? response = await branch.Route(req);

            if (response is not null)
            {
                return response;
            }
        }

        return default;
    }
}