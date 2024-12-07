using System.Text.RegularExpressions;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Func;
using Yaapii.Atoms.Scalar;

namespace Point;

public interface IRqRegex : IRequest
{
    Match Match();

    public sealed class Fake : IRqRegex
    {
        private readonly IRequest req;
        private readonly IFunc<IRqUri, Match> match;

        public Fake(IRequest req, string pattern)
            : this(req, new Regex(pattern, RegexOptions.Compiled))
        {
        }
        
        public Fake(IRequest req, Regex regex)
            : this(req, r => regex.Match(r.Uri().LocalPath))
        {
        }
        
        public Fake(IRequest req, Match match)
            : this(req, _ => match)
        {
        }
        
        public Fake(IRequest req, string pattern, string query)
            : this(req, _ => new Regex(pattern, RegexOptions.Compiled).Match(query))
        {
        }

        private Fake(IRequest req, Func<IRqUri, Match> func)
            : this(req, new FuncOf<IRqUri, Match>(func))
        {
        }

        private Fake(IRequest req, IFunc<IRqUri, Match> match)
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
            return this.match.Invoke(new RqUri(this));
        }
    }
}