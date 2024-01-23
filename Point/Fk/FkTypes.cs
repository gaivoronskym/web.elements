﻿using Point.Rq;
using Point.Rq.Interfaces;

namespace Point.Fk;

public sealed class FkTypes : IFork
{
    private readonly string _type;
    private readonly IResponse _response;
    private const string AcceptAll = "*/*";
    
    public FkTypes(string type, IResponse response)
    {
        _type = type;
        _response = response;
    }

    public Task<IResponse?> Route(IRequest req)
    {
        var acceptHeader = new RqHeaders(req).Headers()["Accept"] ?? "text/html";

        if (acceptHeader.Equals(AcceptAll))
        {
            return Task.FromResult<IResponse?>(_response);
        }
        
        if (acceptHeader.Contains(_type))
        {
            return Task.FromResult<IResponse?>(_response);
        }

        return Task.FromResult<IResponse?>(default);
    }
}