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
    private readonly IPoint _origin;
    private readonly IEnumerable<string> _allowed;

    public PtHeaderCors(IPoint origin, IEnumerable<string> allowed)
    {
        _origin = origin;
        _allowed = allowed;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        var headers = new RqHeaders(req).Headers().Keys;

        foreach (var header in _allowed)
        {
            if (!headers.Contains(header))
            {
                throw new HttpCorsException(
                    HttpStatusCode.Forbidden,
                    new ListOf<string>(
                        new Formatted("Access-Control-Request-Headers: {0}", new Joined(", ", _allowed)).AsString()
                    )
                );
            }
        }

        return new RsWithHeader(
            await _origin.Act(req),
            new Formatted("Access-Control-Request-Headers: {0}", new Joined(", ", _allowed))
        );
    }
}