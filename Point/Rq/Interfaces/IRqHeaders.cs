using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Rq.Interfaces;

public interface IRqHeaders : IRequest
{
    IList<string> Header(string name);
    
    IList<string> Names();
    
    public sealed class Base : IRqHeaders
    {
        private readonly IRequest origin;
    
        public Base(IRequest origin)
        {
            this.origin = origin;
        }
    
        public IEnumerable<string> Head()
        {
            return origin.Head();
        }
    
        public Stream Body()
        {
            return origin.Body();
        }
    
        public IList<string> Header(string name)
        {
            return this.Map()[name];
        }

        public IList<string> Names()
        {
            return Map().Keys.ToList();
        }
    
        private IDictionary<string, IList<string>> Map()
        {
            var heads = new Distinct<string>(
                new Filtered<string>(
                    (item) => new Not(
                        new StartsWith(
                            new TextOf(item),
                            "HTTP/")
                    ).Value(),
                    Head()
                )
            );
    
            var map = new Dictionary<string, IList<string>>();

            foreach (var head in heads)
            {
                var splittedHead = new Split(head, ": ");

                if (splittedHead.Count() != 2)
                {
                    continue;
                }

                var header = new Trimmed(splittedHead.First()).AsString();
                var value = new Trimmed(splittedHead.Last()).AsString();

                if (map.ContainsKey(header))
                {
                    map[header].Add(value);
                }
                else
                {
                    map.Add(header, new List<string> { value });
                }
            }

            return map;
        }
    }
}