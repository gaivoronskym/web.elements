using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

public sealed class RqHref : IRqHref
{
    private readonly IRequest origin;
    private readonly IRqRequestLine requestLine;
    private readonly IRqHeaders headers;

    public RqHref(IRequest origin)
    {
        this.origin = origin;
        this.requestLine = new RequestLine(origin);
        this.headers = new RqHeaders(origin);
    }

    public IEnumerable<string> Head()
    {
        return origin.Head();
    }

    public Stream Body()
    {
        return origin.Body();
    }

    public Href Href()
    {
        var uri = this.requestLine.Uri();
        var hosts = this.headers.Header("Host").ToList();
        var protos = this.headers.Header("x-forwarded-proto").ToList();

        IText host = hosts.Any() ? new Trimmed(new TextOf(hosts[0])) : new TextOf("localhost");
        IText proto = protos.Any() ? new Trimmed(new TextOf(protos[0])) : new TextOf("http");

        var path = uri.StartsWith("/") ? uri : $"/{uri}";
            
        return new Href(
            $"{proto.AsString()}://{host.AsString()}{path}"
        );
    }
}