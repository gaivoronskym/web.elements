﻿using System.Net;
using Point.Exceptions;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Fk;

public sealed class PtFork : IPoint
{
    private readonly IList<IFork> _forks;

    public PtFork(params IFork[] forks)
    {
        _forks = forks;
    }
    
    public async Task<IResponse> Act(IRequest req)
    {
        try
        {
           var res = await new FkPool(_forks).Route(req);

            if (!res.IsEmpty())
            {
                return res.Value();
            }

            return new RsWithStatus(
                HttpStatusCode.NotFound
            );
        }
        catch (HttpCorsException ex)
        {
            return new RsWithHeader(
                new RsWithStatus(
                    ex.Status()
                ),
                ex.Head()
            );
        }
        catch (Exception ex)
        {
            return new RsWithStatus(
                new RsText(ex.Message),
                HttpStatusCode.InternalServerError
            );
        }
    }
}