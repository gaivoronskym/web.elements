using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace Point.Fk;

public sealed class FkParams : IFork
{
    private readonly string name;
    private readonly Regex regex;
    private readonly IPoint point;

    public FkParams(string name, string pattern, IPoint point)
        : this(name, new Regex(pattern, RegexOptions.Compiled), point)
    {
    }

    public FkParams(string name, Regex regex, IPoint point)
    {
        this.name = name;
        this.regex = regex;
        this.point = point;
    }

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        IOpt<IResponse> res;
        var queryParams = new IRqHref.Base(req).Href().Param(this.name);

        if (queryParams.Count > 0 && regex.IsMatch(queryParams[0]))
        {
            res = new Opt<IResponse>(await this.point.Act(req));
        }
        else
        {
            res = new IOpt<IResponse>.Empty();
        }

        return res;
    }
}