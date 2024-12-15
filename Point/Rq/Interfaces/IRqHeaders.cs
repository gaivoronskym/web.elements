using System.Net;
using Point.Exceptions;
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
            var map = this.Map();

            if (map.ContainsKey(name))
            {
                return this.Map()[name];
            }
            
            return new List<string>();
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

    public sealed class Smart : IRqHeaders
    {
        private readonly IRqHeaders origin;

        public Smart(IRequest origin)
            : this(new Base(origin))
        {
        }

        public Smart(IRqHeaders origin)
        {
            this.origin = origin;
        }

        public IEnumerable<string> Head()
        {
            return this.origin.Head();
        }

        public Stream Body()
        {
            return this.origin.Body();
        }

        public IList<string> Header(string name)
        {
            return this.origin.Header(name);
        }

        public IList<string> Names()
        {
            return this.origin.Names();
        }

        public string Single(string name)
        {
            var headers = this.Header(name);
            if (!headers.Any())
            {
                var formatted = string.Join("\r\n", headers);
                throw new HttpException(
                    HttpStatusCode.BadRequest,
                    $"Header '{name}' is mandatory, not found among {formatted}"
                );
            }
            
            return headers.First();
        }

        public string Single(string name, string defaultValue)
        {
            var headers = this.Header(name);
            string value;
            
            if (!headers.Any())
            {
                value = defaultValue;
            }
            else
            {
                value = headers.First();
            }
            
            return value;
        }
    }
}