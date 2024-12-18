using System.Text.RegularExpressions;
using Yaapii.Atoms;
using Yaapii.Atoms.Func;

namespace Web.Elements.Rq;

public sealed class RqRegex : IRqRegex
{
    private readonly IRequest req;
    private readonly IFunc<IRqHref, Match> match;

    public RqRegex(IRequest req, string pattern)
        : this(req, new Regex(pattern, RegexOptions.Compiled))
    {
    }
        
    public RqRegex(IRequest req, Regex regex)
        : this(req, r => regex.Match(r.Href().LocalPath()))
    {
    }
        
    public RqRegex(IRequest req, Match match)
        : this(req, _ => match)
    {
    }
        
    public RqRegex(IRequest req, string pattern, string query)
        : this(req, _ => new Regex(pattern, RegexOptions.Compiled).Match(query))
    {
    }

    private RqRegex(IRequest req, Func<IRqHref, Match> func)
        : this(req, new FuncOf<IRqHref, Match>(func))
    {
    }

    private RqRegex(IRequest req, IFunc<IRqHref, Match> match)
    {
        this.req = req;
        this.match = match;
    }

    public IEnumerable<string> Head()
    {
        return this.req.Head();
    }

    public Stream Body()
    {
        return this.req.Body();
    }

    public Match Match()
    {
        return this.match.Invoke(new RqHref(this));
    }
}