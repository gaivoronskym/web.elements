using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Pt;

public class PtOriginCors : IPoint
{
    private readonly IPoint _origin;
    private readonly IEnumerable<string> _domains;

    public PtOriginCors(IPoint origin, params string[] domains)
    {
        _origin = origin;
        _domains = domains;
    }

    public IResponse Act(IRequest req)
    {
        string domain = new RqHeaders(req).Headers()["domain"];
        if (_domains.Contains(domain))
        {
            return new RsWithHeader(
                _origin.Act(req),
                "Access-Control-Allow-Credentials: true",
                new Formatted("Access-Control-Allow-Origin: {0}", domain).AsString()
            );
        }
        
        throw new HttpCorsException(
            HttpStatusCode.Forbidden,
            new ListOf<string>(
                "Access-Control-Allow-Credentials: false"
            )
        );
    }
}