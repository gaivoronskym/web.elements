using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.Enumerable;

namespace Point.Fk;

public abstract class FkWrap : IFork
{
    private readonly IEnumerable<IFork> forks;

    protected FkWrap(params IFork[] forks)
        : this(new ManyOf<IFork>(forks))
    {
    }

    protected FkWrap(IEnumerable<IFork> forks)
    {
        this.forks = forks;
    }

    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        foreach (var fork in this.forks)
        {
            var res = await fork.Route(req);
            if (res.Has())
            {
                return res;
            }
        }

        return new IOptinal<IResponse>.Empty();
    }
}