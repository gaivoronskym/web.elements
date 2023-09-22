using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using Joined = Yaapii.Atoms.Text.Joined;

namespace Point.Pt.Cors;

public class PtMethodCors : IPoint
{
    private readonly IPoint _origin;
    private readonly IEnumerable<string> _allowed;
    
    public PtMethodCors(IPoint origin, params string[] allowed)
    {
        _origin = origin;
        _allowed = allowed;
    }

    public IResponse Act(IRequest req)
    {
        var method = new RqMethod(req).Method();
        if (_allowed.Contains(method))
        {
            return new RsWithHeader(
                _origin.Act(req),
                new Formatted("Access-Control-Allow-Methods: {0}", new Joined(", ", _allowed)) 
            );
        }

        throw new HttpCorsException(
            HttpStatusCode.MethodNotAllowed,
            new ListOf<string>(
                new Formatted("Access-Control-Allow-Methods: {0}", new Joined(", ", _allowed)).AsString()
            )
        );
    }
}