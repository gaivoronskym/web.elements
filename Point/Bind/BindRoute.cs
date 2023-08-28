using Point.Pt;
using Point.Rq;

namespace Point.Bind;

public class BindRoute : IBind
{
    private readonly string _route;
    private readonly IPoint _point;

    public BindRoute(string route, IPoint point)
    {
        _route = route;
        _point = point;
    }

    public BindRoute(string route, string text)
        : this(route, new PtText(text))
    {
        
    }

    public IResponse? Route(IRequest req)
    {
        var uri = new RqUri(req).Uri();

        if (uri.LocalPath.Equals(_route))
        {
            return _point.Act(req);
        }

        return default;
    }
}