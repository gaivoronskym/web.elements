using System.Net;
using Point.Fk;
using Point.Rq.Interfaces;

namespace Point.Rs;

public sealed class RsFork : IResponse
{
    private readonly IRequest _req;
    private readonly IEnumerable<IFork> _forks;
    
    public RsFork(IRequest req, params IFork[] forks)
    {
        _req = req;
        _forks = forks;
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
        foreach (var fork in _forks)
        {
            var response = AsyncHelper.RunSync(() => fork.Route(_req));
        
            if (response is not null)
            {
                return response;
            }
        }
        
        throw new HttpRequestException("Not Found", null, HttpStatusCode.NotFound);
    }
}