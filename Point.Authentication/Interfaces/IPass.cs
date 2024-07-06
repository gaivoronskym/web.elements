using Point.Rq.Interfaces;

namespace Point.Authentication.Interfaces;

public interface IPass
{
    IOpt<IIdentity> Enter(IRequest req);

    IResponse Exit(IResponse response, IIdentity identity);
}