using System.Net;
using Point.Exceptions;
using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace Point.Fk;

public sealed class PtFork : IPoint
{
    private readonly IEnumerable<IFork> forks;

    public PtFork(params IFork[] forks)
    {
        this.forks = forks;
    }

    public PtFork(IEnumerable<IFork> forks)
    {
        this.forks = forks;
    }
    
    public async Task<IResponse> Act(IRequest req)
    {
        var res = await new FkChain(forks).Route(req);

        if (res.Has())
        {
            return res.Value();
        }

        throw new HttpException(HttpStatusCode.NotFound);
    }
}