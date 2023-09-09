using System.Net;
using Point.Authentication.Interfaces;
using Point.Branch;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Authentication.Branch;

public class BranchAuth : IBranch
{
    private readonly IList<IBranch> _branches;
    private readonly IPass _pass;

    public BranchAuth(IPass pass, params IBranch[] branches)
    {
        _pass = pass;
        _branches = branches;
    }
    
    public IResponse? Route(IRequest req)
    {
        IIdentity identity = _pass.Enter(req);

        if (string.IsNullOrEmpty(identity.Identifier()))
        {
            return new RsWithStatus(HttpStatusCode.Unauthorized);
        }
        
        foreach (var branch in _branches)
        {
            IResponse? response = branch.Route(req);

            if (response is not null)
            {
                return response;
            }
        }

        return default;
    }
}