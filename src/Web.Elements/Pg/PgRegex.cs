using System.Text.RegularExpressions;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Func;

namespace Web.Elements.Pg;

public sealed class PgRegex : IPage
{
    private readonly IPgRegex origin;
    private readonly IFunc<IRequest, Match> match;

    public PgRegex(IPgRegex origin, string pattern, string query)
        : this(origin, new Regex(pattern, RegexOptions.Compiled), query)
    {
    }
        
    public PgRegex(IPgRegex origin, Regex regex, string query)
        : this(origin, () => regex.Match(query))
    {
    }

    public PgRegex(IPgRegex origin, Func<Match> func)
        : this(origin, new FuncOf<IRequest, Match>(func))
    {
    }

    public PgRegex(IPgRegex origin, string pattern)
        : this(origin, new Regex(pattern, RegexOptions.Compiled))
    {
    }

    public PgRegex(IPgRegex origin, Regex regex)
        : this(origin, req => regex.Match(new RqHref(req).Href().LocalPath()))
    {
    }

    public PgRegex(IPgRegex origin, Func<IRequest, Match> func)
        : this(origin, new FuncOf<IRequest, Match>(func))
    {
    }

    public PgRegex(IPgRegex origin, IFunc<IRequest, Match> match)
    {
        this.origin = origin;
        this.match = match;
    }

    public Task<IResponse> Act(IRequest req)
    {
        return this.origin.Act(new RqRegex(req, this.match.Invoke(req)));
    }
}