using Point.Authentication.Interfaces;
using Point.Authentication.Rq;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;

namespace Point.Authentication.Pt;

public sealed class PtAuth : IPoint
{
    private readonly IPoint _origin;
    private readonly IPass _pass;
    private readonly string _header;

    public PtAuth(IPoint origin, IPass pass, string header)
    {
        _origin = origin;
        _pass = pass;
        _header = header;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        IIdentity identity = _pass.Enter(req);

        if (string.IsNullOrEmpty(identity.Identifier()))
        {
            IRequest wrap = new RqWithoutHeader(req, _header);
            return await _origin.Act(wrap);
        }
        
        return await Act(req, identity);
    }

    private async Task<IResponse> Act(IRequest req, IIdentity identity)
    {
        IRequest wrap = new RqWithoutHeader(req, _header);
        if(identity is not Anonymous)
        {
            wrap = new RqWithAuth(wrap, identity, _header);
        }

        return _pass.Exit(await _origin.Act(wrap), identity);
    }
}