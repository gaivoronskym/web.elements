using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.List;

namespace Web.Elements.Ps;

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

    public IOptional<IIdentity> Enter(IRequest req)
    {
        foreach (var pass in passes)
        {
            var user = pass.Enter(req);
            if (user.Has())
            {
                return user;
            }
        }

        return new IOptional<IIdentity>.Empty();
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