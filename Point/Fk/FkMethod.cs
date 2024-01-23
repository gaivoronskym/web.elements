﻿using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;

namespace Point.Fk;

public sealed class FkMethod : IFork
{
    private readonly IList<string> _methods;
    private readonly IPoint _point;
    
    public FkMethod(string method, IPoint point)
        : this(new ListOf<string>(method), point)
    {
        
    }

    public FkMethod(IList<string> methods, IPoint point)
    {
        _methods = methods;
        _point = point;
    }

    public async Task<IResponse?> Route(IRequest req)
    {
        var method = new RqMethod(req).Method();

        if (_methods.Contains(method))
        {
            return await _point.Act(req);
        }
        
        return default;
    }
}