using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Pt.Cors;

public sealed class PtOriginCors : IPoint
{
    private readonly IPoint point;
    private readonly IEnumerable<string> allowed;

    public PtOriginCors(IPoint point, params string[] allowed)
    {
        this.point = point;
        this.allowed = allowed;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        var origin = new IRqHeaders.Base(req).Header("Origin")[0];
        if (allowed.Contains(origin))
        {
            return new RsWithHeader(
                await this.point.Act(req),
                new Formatted("Access-Control-Allow-Origin: {0}", origin)
            );
        }

        throw new HttpCorsException(
            HttpStatusCode.Forbidden,
            new ListOf<string>(
                new Formatted("Access-Control-Allow-Origin: {0}", origin).AsString()
            )
        );
    }
}