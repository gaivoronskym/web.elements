﻿using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Enumerable;

namespace Web.Elements.Fk;

public sealed class FkChain : IFork
{
    private readonly IEnumerable<IFork> forks;

    public FkChain(params IFork[] forks)
        : this(new ManyOf<IFork>(forks))
    {
    }
    
    public FkChain(IEnumerable<IFork> forks)
    {
        this.forks = forks;
    }
    
    public async Task<IOptional<IResponse>> Route(IRequest req)
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

        return new IOptional<IResponse>.Empty();
    }
}