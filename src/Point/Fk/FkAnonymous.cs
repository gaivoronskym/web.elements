﻿using Point.Pt;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Fk;

public sealed class FkAnonymous : IFork
{
    private readonly IScalar<IPoint> point;

    public FkAnonymous(IPoint point)
        : this(new Live<IPoint>(() => point))
    {
    }

    public FkAnonymous(IScalar<IPoint> point)
    {
        this.point = point;
    }
    
    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        var identity = new RqAuth(req).Identity();
        if (string.IsNullOrEmpty(identity.Identifier()))
        {
            var res = await point.Value().Act(req);
            return new Opt<IResponse>(res);
        }

        return new IOpt<IResponse>.Empty();
    }
}