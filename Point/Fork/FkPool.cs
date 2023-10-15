using Point.Rq.Interfaces;

namespace Point.Fork;

public sealed class FkPool : IFork
{
    private readonly IList<IFork> _forks;

    public FkPool(IList<IFork> branches)
    {
        _forks = branches;
    }

    public FkPool(params IFork[] branches)
    {
        _forks = branches;
    }
    
    public async Task<IResponse?> Route(IRequest req)
    {
        foreach (var fork in _forks)
        {
            IResponse? response = await fork.Route(req);

            if (response is not null)
            {
                return response;
            }
        }

        return default;
    }
}