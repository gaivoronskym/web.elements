using System.Net;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Branch;

public class PtBranch : IPoint
{
    private readonly IList<IBranch> _binds;

    public PtBranch(params IBranch[] binds)
    {
        _binds = binds;
    }
    
    public IResponse Act(IRequest req)
    {
        try
        {
            IResponse? response = new BranchPool(_binds).Route(req);

            if (response is not null)
            {
                return response;
            }

            return new RsWithStatus(
                new RsText("Not found"),
                HttpStatusCode.NotFound
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