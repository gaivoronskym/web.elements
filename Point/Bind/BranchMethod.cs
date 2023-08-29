using System.Text;
using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using StringJoined = Yaapii.Atoms.Enumerable.Joined<string>;

namespace Point.Bind;

public class BranchMethod : IBranch
{
    private readonly IList<string> _methods;
    private readonly IPoint _point;
    private readonly string _pattern;
    
    private readonly Regex _pathRegex = new Regex(@"((?<static>[^/]+))(?<param>(((/({(?<data>[^}/:]+))?)(((:(?<type>[^}/]+))?)}))?))", RegexOptions.Compiled);

    public BranchMethod(string method, string pattern, IPoint point)
        : this(new ListOf<string>(method), pattern, point)
    {
        
    }

    public BranchMethod(IList<string> methods, string pattern, IPoint point)
    {
        _methods = methods;
        _point = point;
        _pattern = pattern;
    }

    public IResponse? Route(IRequest req)
    {
        var method = new RqMethod(req).Method();
        var uri = new RqUri(req).Uri();

        var routeParams = BuildRouteParamHead(_pattern);

        var route = _pattern;
        
        foreach (Match match in _pathRegex.Matches(route))
        {
            route = route.Replace(match.Groups["param"].Value, $"/{match.Groups["type"]}");
        }
        
        //for full matching
        route += "$";

        if (_methods.Contains(method) && new Regex(route).IsMatch(uri.LocalPath))
        {
            return _point.Act(
                new RequestOf(
                    new StringJoined(
                        req.Head(),
                        routeParams
                    ),
                    req.Body()
                )
            );
        }
        
        return default;
    }
    
    private string BuildRouteParamHead(string pattern)
    {
        StringBuilder param = new StringBuilder();

        var matches = _pathRegex.Matches(pattern);

        var segments = new ListOf<string>(
            new Split(
                pattern,
                "/"
            )
        );
        
        for (var i = 0; i < matches.Count; i++)
        {
            var match = matches[i];

            var key = match.Groups["data"].Value;
            
            if (!string.IsNullOrEmpty(key))
            {
                var index = segments.IndexOf(segments.First(x => x.Contains(key)));
                
                param.Append($"{match.Groups["data"].Value};{index},");
            }
        }

        if (param.Length == 0)
        {
            return string.Empty;
        }

        return $"path:{param}";
    }
}