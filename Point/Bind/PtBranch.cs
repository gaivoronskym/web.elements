using System.Net;
using Point.Pt;
using Point.Rq.Interfaces;

namespace Point.Bind;

public class PtBranch : IPoint
{
    private readonly IList<IBranch> _binds;

    public PtBranch(params IBranch[] binds)
    {
        _binds = binds;
    }
    
    public IResponse Act(IRequest req)
    {
        IResponse? response = new BranchPool(_binds).Route(req);

        if (response is not null)
        {
            return response;
        }

        throw new HttpRequestException("Not found", null, HttpStatusCode.NotFound);
    }
}