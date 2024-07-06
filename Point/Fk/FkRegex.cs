using System.Text.RegularExpressions;
using Point.Extensions;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Map;
using StringJoined = Yaapii.Atoms.Enumerable.Joined<string>;

namespace Point.Fk;

public sealed class FkRegex : IFork
{
    private readonly string pattern;
    private readonly IPoint point;
    
    private readonly Regex pathRegex = new Regex(@"((?<static>[^/]+))(?<param>(((/({(?<data>[^}/:]+))?)(((:(?<type>[^}/]+))?)}))?))", RegexOptions.Compiled);

    public FkRegex(string pattern, string text)
        : this(pattern, new PtText(text))
    {
    }

    public FkRegex(string pattern, IPoint point)
    {
        this.pattern = pattern;
        this.point = point;
    }

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        var uri = new RqUri(req).Uri();

        var regex = new Regex(pattern, RegexOptions.Compiled);
        if (regex.IsMatch(uri.LocalPath))
        {
            var map = new List<IKvp>();
            
            foreach (Match match in regex.Matches(uri.LocalPath))
            {
                foreach (Group matchGroup in match.Groups)
                {
                    if (!matchGroup.Name.IsEmpty())
                    {
                        map.Add(
                            new KvpOf(
                                matchGroup.Name,
                                matchGroup.Value
                            )
                        );
                    }
                }
            }

            var res = await point.Act(
                new RqUri(
                    req,
                    map
                )
            );
            
            return new Opt<IResponse>(res);
        }

        return new IOpt<IResponse>.Empty();
    }
}