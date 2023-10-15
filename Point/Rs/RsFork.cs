using System.Net;
using Nito.AsyncEx;
using Point.Fork;
using Point.Rq.Interfaces;

namespace Point.Rs;

public sealed class RsFork : RsWrap
{
    public RsFork(IRequest req, params IFork[] forks)
        : base(
            new ResponseOf(
                () => Pick(req, forks).Head(),
                () => Pick(req, forks).Body()
            )
        )
    {
    }

    private static IResponse Pick(IRequest req, IEnumerable<IFork> forks)
    {
        foreach (var fork in forks)
        {
            var response = AsyncContext.Run(() => fork.Route(req));

            if (response is not null)
            {
                return response;
            }
        }

        throw new HttpRequestException("Not Found", null, HttpStatusCode.NotFound);
    } 
}