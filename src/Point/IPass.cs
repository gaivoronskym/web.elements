using Point.Rq;
using Point.Rs;

namespace Point;

public interface IPass
{
    IOptinal<IIdentity> Enter(IRequest req);

    IResponse Exit(IResponse response, IIdentity identity);
}