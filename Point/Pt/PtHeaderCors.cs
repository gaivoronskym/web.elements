using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;

namespace Point.Pt;

public class PtHeaderCors : IPoint
{
    private readonly IPoint _origin;
    private readonly IEnumerable<string> _allowed;

    public PtHeaderCors(IPoint origin, IEnumerable<string> allowed)
    {
        _origin = origin;
        _allowed = allowed;
    }

    public IResponse Act(IRequest req)
    {
        var headers = new RqHeaders(req).Headers().Keys;

        foreach (var header in _allowed)
        {
            if (!headers.Contains(header))
            {
                throw new HttpCorsException(
                    HttpStatusCode.Forbidden,
                    new ListOf<string>(
                        "Access-Control-Allow-Credentials: false"
                    )
                );
            }
        }
        
        return _origin.Act(req);
    }
}