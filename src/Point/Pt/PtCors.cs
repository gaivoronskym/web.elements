using System.Net;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Pt;

public sealed class PtCors : IPoint
{
    private readonly IPoint point;
    private readonly IEnumerable<string> allowed;

    public PtCors(IPoint point, params string[] allowed)
    {
        this.point = point;
        this.allowed = allowed;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        var origin = new IRqHeaders.Smart(req).Single("Origin", "");
        
        IResponse response;
        
        if (allowed.Contains(origin))
        {
            response = new RsWithHeader(
                await this.point.Act(req),
                new TextOf("Access-Control-Allow-Credentials: true"),
                new TextOf("Access-Control-Allow-Methods: OPTIONS, GET, POST, PUT, DELETE, HEAD"),
                new Formatted("Access-Control-Allow-Origin: {0}", origin)
            );
        }
        else
        {
            response = new RsWithHeaders(
                new RsWithStatus(
                    HttpStatusCode.Forbidden
                ),
                "Access-Control-Allow-Credentials: false"
            );
        }

        return response;
    }
}