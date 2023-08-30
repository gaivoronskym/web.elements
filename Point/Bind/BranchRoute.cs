using System.Text;
using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using StringJoined = Yaapii.Atoms.Enumerable.Joined<string>;


namespace Point.Bind;

public class BranchRoute : IBranch
{
    private readonly string _pattern;
    private readonly IPoint _point;
    
    private readonly Regex _pathRegex = new Regex(@"((?<static>[^/]+))(?<param>(((/({(?<data>[^}/:]+))?)(((:(?<type>[^}/]+))?)}))?))", RegexOptions.Compiled);

    public BranchRoute(string pattern, IPoint point)
    {
        _pattern = pattern;
        _point = point;
    }

    public BranchRoute(string pattern, string text)
        : this(pattern, new PtText(text))
    {
        
    }

    public IResponse? Route(IRequest req)
    {
        var uri = new RqUri(req).Uri();

        var routeParams = BuildRouteParamHead(_pattern);

        var route = _pattern;
        
        foreach (Match match in _pathRegex.Matches(route))
        {
            if (!string.IsNullOrEmpty(match.Groups["param"].Value))
            {
                route = route.Replace(match.Groups["param"].Value, $"/{match.Groups["type"]}");
            }
        }
        
        //for full matching
        route += "$";

        if (new Regex(route, RegexOptions.Compiled).IsMatch(uri.LocalPath))
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