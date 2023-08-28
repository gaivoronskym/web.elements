using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;

namespace Point.Bind;

public class BranchRoute : IBranch
{
    private readonly Regex _pattern;
    private readonly IPoint _point;

    public BranchRoute(Regex pattern, IPoint point)
    {
        _pattern = pattern;
        _point = point;
    }

    public BranchRoute(string pattern, IPoint point)
      : this (new Regex(pattern), point)
    {
        
    }

    public BranchRoute(string pattern, string text)
        : this(pattern, new PtText(text))
    {
        
    }

    public IResponse? Route(IRequest req)
    {
        var uri = new RqUri(req).Uri();

        if (_pattern.IsMatch(uri.LocalPath))
        {
            return _point.Act(req);
        }

        return default;
    }
}