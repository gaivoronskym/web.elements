using System.Text.RegularExpressions;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public sealed class FkParams : IFork
{
    private readonly string name;
    private readonly Regex regex;
    private readonly IPage page;

    public FkParams(string name, string pattern, IPage page)
        : this(name, new Regex(pattern, RegexOptions.Compiled), page)
    {
    }

    public FkParams(string name, Regex regex, IPage page)
    {
        this.name = name;
        this.regex = regex;
        this.page = page;
    }

    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        IOptinal<IResponse> res;
        var queryParams = new IRqHref.Base(req).Href().Param(this.name);

        if (queryParams.Count > 0 && regex.IsMatch(queryParams[0]))
        {
            res = new Optinal<IResponse>(await this.page.Act(req));
        }
        else
        {
            res = new IOptinal<IResponse>.Empty();
        }

        return res;
    }
}