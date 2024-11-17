using Microsoft.AspNetCore.Http;
using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Point.AspNet;

public sealed class RsContextPrint : IResponse
{
    private readonly IResponse origin;
    private readonly IScalar<int> statusCode;
    private readonly RsPrint rsPrint;
    private readonly IEnumerable<HeadLine> headers;

    public RsContextPrint(IResponse origin)
    {
        this.origin = origin;
        this.statusCode = new StatusCodeOf(origin);
        this.rsPrint = new RsPrint(origin);

        this.headers = new Mapped<string, HeadLine>(
            i => new HeadLine(i),
            new Filtered<string>(
                i => new Not(new HttpMatch(i)).Value(),
                this.origin.Head()
            )
        );
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
        httpResponse.StatusCode = this.statusCode.Value();

        foreach (var head in this.headers)
        {
            httpResponse.Headers.Add(head.Key(), head.Value());
        }

        this.rsPrint.PrintBody(httpResponse.BodyWriter.AsStream());
    }
}