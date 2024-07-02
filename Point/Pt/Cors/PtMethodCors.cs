using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using Joined = Yaapii.Atoms.Text.Joined;

namespace Point.Pt.Cors;

public sealed class PtMethodCors : IPoint
{
    private readonly IPoint origin;
    private readonly IEnumerable<string> allowed;
    
    public PtMethodCors(IPoint origin, params string[] allowed)
    {
        this.origin = origin;
        this.allowed = allowed;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        var method = new RqMethod(req).Method();
        if (allowed.Contains(method))
        {
            return new RsWithHeader(
                await origin.Act(req),
                new Formatted("Access-Control-Allow-Methods: {0}", new Joined(", ", allowed)) 
            );
        }

        throw new HttpCorsException(
            HttpStatusCode.MethodNotAllowed,
            new ListOf<string>(
                new Formatted("Access-Control-Allow-Methods: {0}", new Joined(", ", allowed)).AsString()
            )
        );
    }
}