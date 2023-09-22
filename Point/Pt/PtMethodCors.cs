using System.Net;
using Point.Exceptions;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.Text;
using Joined = Yaapii.Atoms.Text.Joined;

namespace Point.Pt;

public class PtMethodCors : IPoint
{
    private readonly IPoint _origin;
    private readonly IEnumerable<string> _methods;
    
    public PtMethodCors(IPoint origin, params string[] methods)
    {
        _origin = origin;
        _methods = methods;
    }

    public IResponse Act(IRequest req)
    {
        var method = new RqMethod(req).Method();
        if (_methods.Contains(method))
        {
            return new RsWithHeader(
                _origin.Act(req),
                "Access-Control-Allow-Credentials: true",
                new Formatted("Access-Control-Allow-Methods: {0}", new Joined(", ", _methods)) 
            );
        }

        throw new HttpCorsException(
            HttpStatusCode.MethodNotAllowed
        );
    }
}