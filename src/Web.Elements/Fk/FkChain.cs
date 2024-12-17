using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public sealed class FkChain : IFork
{
    private readonly IEnumerable<IFork> forks;

    public FkChain(IEnumerable<IFork> forks)
    {
        this.forks = forks;
    }

    public FkChain(params IFork[] forks)
    {
        this.forks = forks;
    }
    
    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        foreach (var fork in forks)
        {
            var res = await fork.Route(req);

            if (!res.Has())
            {
                continue;
            }

            return res;
        }

        return new IOptinal<IResponse>.Empty();
    }
}