using System.Net;
using Point.Pt;

namespace Point.Bind;

public class PtBind : IPoint
{
    private readonly IList<IBind> _binds;

    public PtBind(params IBind[] binds)
    {
        _binds = binds;
    }
    
    public IResponse Act(IRequest req)
    {
        IResponse? response = new BindChain(_binds).Route(req);

        if (response is not null)
        {
            return response;
        }

        throw new HttpRequestException("Not found", null, HttpStatusCode.NotFound);
    }
}