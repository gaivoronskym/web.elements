using System.Net;
using Web.Elements.Exceptions;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

public sealed class RqHeaders : IRqHeaders
{
    private readonly IRequest origin;
    
    public RqHeaders(IRequest origin)
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