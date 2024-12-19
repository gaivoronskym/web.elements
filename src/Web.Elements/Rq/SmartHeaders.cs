using System.Net;
using Web.Elements.Exceptions;

namespace Web.Elements.Rq;

public sealed class SmartHeaders : IRqHeaders
{
    private readonly IRqHeaders origin;

    public SmartHeaders(IRequest origin)
        : this(new RqHeaders(origin))
    {
    }

    public SmartHeaders(IRqHeaders origin)
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