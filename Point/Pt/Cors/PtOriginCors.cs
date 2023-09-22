using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Pt.Cors;

public class PtOriginCors : IPoint
{
    private readonly IPoint _origin;
    private readonly IEnumerable<string> _allowed;

    public PtOriginCors(IPoint origin, params string[] allowed)
    {
        _origin = origin;
        _allowed = allowed;
    }

    public IResponse Act(IRequest req)
    {
        string origin = new RqHeaders(req).Headers()["Origin"];
        if (_allowed.Contains(origin))
        {
            return new RsWithHeader(
                _origin.Act(req),
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