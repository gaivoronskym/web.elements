using System.Net;
using Nito.AsyncEx;
using Point.Branch;
using Point.Rq.Interfaces;

namespace Point.Rs;

public sealed class RsBranch : RsWrap
{
    public RsBranch(IRequest req, params IBranch[] branches)
        : base(
            new ResponseOf(
                () => Pick(req, branches).Head(),
                () => Pick(req, branches).Body()
            )
        )
    {
    }

    private static IResponse Pick(IRequest req, IEnumerable<IBranch> branches)
    {
        foreach (var branch in branches)
        {
            var response = AsyncContext.Run(() => branch.Route(req));

            if (response is not null)
            {
                return response;
            }
        }

        throw new HttpRequestException("Not Found", null, HttpStatusCode.NotFound);
    } 
}