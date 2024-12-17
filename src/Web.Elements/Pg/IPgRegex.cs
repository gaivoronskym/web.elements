using System.Text.RegularExpressions;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Func;

namespace Web.Elements.Pg;

public interface IPgRegex
{
    Task<IResponse> Act(IRqRegex req);

    public sealed class Fake : IPage
    {
        private readonly IPgRegex origin;
        private readonly IFunc<IRequest, Match> match;

        public Fake(IPgRegex origin, string pattern, string query)
            : this(origin, new Regex(pattern, RegexOptions.Compiled), query)
        {
        }
        
        public Fake(IPgRegex origin, Regex regex, string query)
            : this(origin, () => regex.Match(query))
        {
        }

        public Fake(IPgRegex origin, Func<Match> func)
            : this(origin, new FuncOf<IRequest, Match>(func))
        {
        }

        public Fake(IPgRegex origin, string pattern)
            : this(origin, new Regex(pattern, RegexOptions.Compiled))
        {
        }

        public Fake(IPgRegex origin, Regex regex)
            : this(origin, req => regex.Match(new IRqHref.Base(req).Href().LocalPath()))
        {
        }

        public Fake(IPgRegex origin, Func<IRequest, Match> func)
            : this(origin, new FuncOf<IRequest, Match>(func))
        {
        }

        public Fake(IPgRegex origin, IFunc<IRequest, Match> match)
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