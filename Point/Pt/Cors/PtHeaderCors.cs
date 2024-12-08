using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using Joined = Yaapii.Atoms.Text.Joined;

namespace Point.Pt.Cors;

public sealed class PtHeaderCors : IPoint
{
    private readonly IPoint origin;
    private readonly IEnumerable<string> allowed;

    public PtHeaderCors(IPoint origin, IEnumerable<string> allowed)
    {
        this.origin = origin;
        this.allowed = allowed;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        var headers = new IRqHeaders.Base(req).Names();

        foreach (var header in allowed)
        {
            if (!headers.Contains(header))
            {
                throw new HttpCorsException(
                    HttpStatusCode.Forbidden,
                    new ListOf<string>(
                        new Formatted("Access-Control-Request-Headers: {0}", new Joined(", ", allowed)).AsString()
                    )
                );
            }
        }

        return new RsWithHeader(
            await origin.Act(req),
            new Formatted("Access-Control-Request-Headers: {0}", new Joined(", ", allowed))
        );
    }
}