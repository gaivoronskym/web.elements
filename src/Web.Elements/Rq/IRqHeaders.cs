using System.Net;
using Web.Elements.Exceptions;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

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
            
            var key = new Lower(new TextOf(name)).AsString();

            if (map.ContainsKey(key))
            {
                return this.Map()[key];
            }
            
            return new List<string>();
        }

        public IList<string> Names()
        {
            return Map().Keys.ToList();
        }
    
        private IDictionary<string, IList<string>> Map()
        {
            var head = this.Head().ToList();

            if (!head.Any())
            {
                throw new HttpException(
                    HttpStatusCode.BadRequest,
                    "A valid request must contains at least one line"
                );
            }
            
            var map = new Dictionary<string, IList<string>>();

            foreach (var line in head.Skip(1))
            {
                var parts = line.Split(':', 2);
                if (parts.Length < 2)
                {
                    throw new HttpException(
                        HttpStatusCode.BadRequest,
                        $"Invalid HTTP header '{line}'"
                    );
                }

                var key = new Lower(new Trimmed(new TextOf(parts[0]))).AsString();

                if (!map.ContainsKey(key))
                {
                    map.Add(key, new List<string>());
                }
                
                map[key].Add(new Trimmed(new TextOf(parts[1])).AsString());
                
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