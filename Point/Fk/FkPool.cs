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
    
    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        foreach (var fork in _forks)
        {
            var res = await fork.Route(req);

            if (res.IsEmpty())
            {
                continue;
            }

            return res;
        }

        return new IOpt<IResponse>.Empty();
    }
}