using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.List;

namespace Point.Ps;

public sealed class PsChain : IPass
{
    private readonly IEnumerable<IPass> passes;

    public PsChain(params IPass[] passes)
        : this(new ListOf<IPass>(passes))
    {
    }

    public PsChain(IEnumerable<IPass> passes)
    {
        this.passes = passes;
    }

    public IOpt<IIdentity> Enter(IRequest req)
    {
        foreach (var pass in passes)
        {
            var user = pass.Enter(req);
            if (user.Has())
            {
                return user;
            }
        }

        return new IOpt<IIdentity>.Empty();
    }

    public IResponse Exit(IResponse response, IIdentity identity)
    {
        var res = response;
        foreach (var pass in passes)
        {
            res = pass.Exit(res, identity);
        }

        return res;
    }
}