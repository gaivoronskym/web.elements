using System.Net;
using Point.Pt;

namespace Point.Bind;

public class PtFork : IPoint
{
    private readonly IList<IBind> _forks;

    public PtFork(params IBind[] forks)
    {
        _forks = forks;
    }
    
    public IResponse Act(IRequest req)
    {
        IResponse? response = new BdChain(this._forks).Route(req);

        if (response is not null)
        {
            return response;
        }

        throw new HttpRequestException("Not found", null, HttpStatusCode.NotFound);
    }
}