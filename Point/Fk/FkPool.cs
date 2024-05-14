using Point.Rq.Interfaces;

namespace Point.Fk;

public sealed class FkPool : IFork
{
    private readonly IList<IFork> _forks;

    public FkPool(IList<IFork> forks)
    {
        _forks = forks;
    }

    public FkPool(params IFork[] forks)
    {
        _forks = forks;
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