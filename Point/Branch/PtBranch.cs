using System.Net;
using Point.Exceptions;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Branch;

public sealed class PtBranch : IPoint
{
    private readonly IList<IBranch> _branches;

    public PtBranch(params IBranch[] branches)
    {
        _branches = branches;
    }
    
    public IResponse Act(IRequest req)
    {
        try
        {
            IResponse? response = new BranchPool(_branches).Route(req);

            if (response is not null)
            {
                return response;
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