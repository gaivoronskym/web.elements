using Microsoft.AspNetCore.Http;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Asp;

public sealed class RsContextPrint : IResponse
{
    private readonly IRsStatus origin;
    private readonly RsPrint rsPrint;

    public RsContextPrint(IResponse origin)
    {
        this.origin = new IRsStatus.Base(origin);
        this.rsPrint = new RsPrint(origin);
    }

    public IEnumerable<string> Head()
    {
        return this.origin.Head();
    }

    public Stream Body()
    {
        return this.origin.Body();
    }

    public void Print(HttpResponse httpResponse)
    {
        httpResponse.StatusCode = this.origin.Status();

        foreach (var head in this.Head().Skip(1).ToList())
        {
            var parts = head.Split(':', 2);
            
            httpResponse.Headers.Append(parts[0], new Trimmed(new TextOf(parts[1])).AsString());
        }

        this.rsPrint.PrintBody(httpResponse.BodyWriter.AsStream());
    }
}