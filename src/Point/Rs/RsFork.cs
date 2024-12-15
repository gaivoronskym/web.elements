using System.Net;
using Point.Fk;
using Point.Rq.Interfaces;

namespace Point.Rs;

public sealed class RsFork : IResponse
{
    private readonly IRequest req;
    private readonly IEnumerable<IFork> forks;
    
    public RsFork(IRequest req, params IFork[] forks)
    {
        this.req = req;
        this.forks = forks;
    }

    public IEnumerable<string> Head()
    {
        return Pick().Head();
    }

    public Stream Body()
    {
        return Pick().Body();
    }

    private IResponse Pick()
    {
        try
        {
            foreach (var fork in forks)
            {
                var res = AsyncHelper.RunSync(() => fork.Route(req));

                return res.Value();
            }

            throw new HttpRequestException("Not Found", null, HttpStatusCode.NotFound);
        }
        catch (Exception)
        {
            throw new HttpRequestException("Not Found", null, HttpStatusCode.NotFound);
        }
    }
}