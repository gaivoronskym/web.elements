using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Yaapii.Atoms.List;

namespace Point.Bind;

public class BunchMethod : IBunch
{
    private readonly IList<string> _methods;
    private readonly Regex _pattern;
    private readonly IPoint _point;

    public BunchMethod(string method, string pattern, IPoint point)
        : this(method, new Regex(pattern), point)
    {
        
    }

    public BunchMethod(string method, Regex pattern, IPoint point)
        : this(new ListOf<string>(method), pattern, point)
    {
        
    }

    public BunchMethod(IList<string> methods, Regex pattern, IPoint point)
    {
        _methods = methods;
        _point = point;
        _pattern = pattern;
    }

    public IResponse? Route(IRequest req)
    {
        var method = new RqMethod(req).Method();
        var uri = new RqUri(req).Uri();

        if (_methods.Contains(method) && _pattern.IsMatch(uri.LocalPath))
        {
            return _point.Act(req);
        }

        return default;
    }
}