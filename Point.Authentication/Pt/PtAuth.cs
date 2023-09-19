using System.Net;
using Point.Authentication.Interfaces;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Authentication.Pt;

public sealed class PtAuth : IPoint
{
    private readonly IPoint _origin;
    private readonly IPass _pass;

    public PtAuth(IPoint origin, IPass pass)
    {
        _origin = origin;
        _pass = pass;
    }

    public IResponse Act(IRequest req)
    {
        IIdentity identity = _pass.Enter(req);

        if (string.IsNullOrEmpty(identity.Identifier()))
        {
            return new RsWithStatus(HttpStatusCode.Unauthorized);
        }
        
        return _origin.Act(req);
    }
}