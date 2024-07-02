using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using StringJoined = Yaapii.Atoms.Enumerable.Joined<string>;


namespace Point.Fk;

public sealed class FkRoute : IFork
{
    private readonly string pattern;
    private readonly IPoint point;
    
    private const string RouteParamKey = "Route56321-";
    
    private readonly Regex pathRegex = new Regex(@"((?<static>[^/]+))(?<param>(((/({(?<data>[^}/:]+))?)(((:(?<type>[^}/]+))?)}))?))", RegexOptions.Compiled);

    public FkRoute(string pattern, string text)
        : this(pattern, new PtText(text))
    {
    }

    public FkRoute(string pattern, IPoint point)
    {
        this.pattern = pattern;
        this.point = point;
    }

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        var uri = new RqUri(req).Uri();
        
        var route = pattern;
        
        foreach (Match match in pathRegex.Matches(route))
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
            var routeParams = BuildRouteParamHead(pattern, uri);

            var res = await point.Act(
                new RequestOf(
                    new StringJoined(
                        req.Head(),
                        routeParams
                    ),
                    req.Body()
                )
            );
            return new Opt<IResponse>(res);
        }

        return new IOpt<IResponse>.Empty();
    }
    
    private IEnumerable<string> BuildRouteParamHead(string pattern, Uri uri)
    {
        IList<string> param = new List<string>();

        var matches = pathRegex.Matches(pattern);

        if (!matches.Any())
        {
            return new ListOf<string>();
        }

        var pathSegments = new ListOf<string>(
            new Split(
                uri.LocalPath,
                "/"
            )
        );
        
        for (var i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            if (string.IsNullOrEmpty(match.Groups["data"].Value))
            {
                continue;
            }
            
            var key = string.Concat("{" + match.Groups["data"], ":", match.Groups["type"] + "}");
            pattern = pattern.Replace(key, match.Groups["data"].Value);
        }

        var patternSegments = new ListOf<string>(
            new Split(
                pattern,
                "/"
            )
        );

        if (patternSegments.Count() != pathSegments.Count())
        {
            return new ListOf<string>();
        }

        var data = new Mapped<Match, string>(
            (match) => match.Groups["data"].Value,
            matches
        );
        
        for (var i = 0; i < patternSegments.Count; i++)
        {
            var patternSegment = patternSegments[i];
            var pathSegment = pathSegments[i];

            if (!data.Contains(patternSegment))
            {
                continue;
            }
            
            param.Add($"{RouteParamKey}{patternSegment}: {pathSegment}");
        }

        if (!param.Any())
        {
            return new ListOf<string>();
        }

        return param;
    }
}