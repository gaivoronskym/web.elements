using System.Net;
using Web.Elements.Exceptions;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Enumerable;

namespace Web.Elements.Fk;

public sealed class PgFork : IPage
{
    private readonly IFork fork;

    public PgFork(params IFork[] forks)
        : this(new ManyOf<IFork>(forks))
    {
    }

    public PgFork(IEnumerable<IFork> forks)
        : this(new FkChain(forks))
    {
    }
    
    public PgFork(IFork fork)
    {
        this.fork = fork;
    }
    
    public async Task<IResponse> Act(IRequest req)
    {
        var res = await this.fork.Route(req);

        if (res.Has())
        {
            return res.Value();
        }

        throw new HttpException(HttpStatusCode.NotFound);
    }
}