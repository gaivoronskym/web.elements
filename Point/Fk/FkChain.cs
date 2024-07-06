using Point.Rq.Interfaces;

namespace Point.Fk;

public sealed class FkPool : IFork
{
    private readonly IEnumerable<IFork> forks;

    public FkPool(IEnumerable<IFork> forks)
    {
        this.forks = forks;
    }

    public FkPool(params IFork[] forks)
    {
        this.forks = forks;
    }
    
    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        foreach (var fork in forks)
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