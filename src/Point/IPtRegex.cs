using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Func;

namespace Point;

public interface IPtRegex
{
    Task<IResponse> Act(IRqRegex req);

    public sealed class Fake : IPoint
    {
        private readonly IPtRegex origin;
        private readonly IFunc<IRequest, Match> match;

        public Fake(IPtRegex origin, string pattern, string query)
            : this(origin, new Regex(pattern, RegexOptions.Compiled), query)
        {
        }
        
        public Fake(IPtRegex origin, Regex regex, string query)
            : this(origin, () => regex.Match(query))
        {
        }

        public Fake(IPtRegex origin, Func<Match> func)
            : this(origin, new FuncOf<IRequest, Match>(func))
        {
        }

        public Fake(IPtRegex origin, string pattern)
            : this(origin, new Regex(pattern, RegexOptions.Compiled))
        {
        }

        public Fake(IPtRegex origin, Regex regex)
            : this(origin, req => regex.Match(new IRqHref.Base(req).Href().LocalPath()))
        {
        }

        public Fake(IPtRegex origin, Func<IRequest, Match> func)
            : this(origin, new FuncOf<IRequest, Match>(func))
        {
        }

        public Fake(IPtRegex origin, IFunc<IRequest, Match> match)
        {
            this.origin = origin;
            this.match = match;
        }

        public Task<IResponse> Act(IRequest req)
        {
            return this.origin.Act(new IRqRegex.Fake(req, this.match.Invoke(req)));
        }
    }
}