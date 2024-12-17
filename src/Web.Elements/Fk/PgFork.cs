using System.Net;
using Web.Elements.Exceptions;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public sealed class PgFork : IPage
{
    private readonly IEnumerable<IFork> forks;

    public PgFork(params IFork[] forks)
    {
        this.forks = forks;
    }

    public PgFork(IEnumerable<IFork> forks)
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