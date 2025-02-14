﻿using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Enumerable;

namespace Web.Elements.Fk;

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

    public async Task<IOptional<IResponse>> Route(IRequest req)
    {
        foreach (var fork in this.forks)
        {
            var res = await fork.Route(req);
            if (res.Has())
            {
                return res;
            }
        }

        return new IOptional<IResponse>.Empty();
    }
}