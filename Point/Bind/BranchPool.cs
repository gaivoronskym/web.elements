using Point.Rq.Interfaces;

namespace Point.Bind;

public class BranchPool : IBranch
{
    private readonly IList<IBranch> _binds;

    public BranchPool(IList<IBranch> binds)
    {
        _binds = binds;
    }

    public IResponse? Route(IRequest req)
    {
        foreach (var fork in _binds)
        {
            IResponse? response = fork.Route(req);

            if (response is not null)
            {
                return response;
            }
        }

        return default;
    }
}