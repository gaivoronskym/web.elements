using Point.Rq.Interfaces;

namespace Point.Authentication.Interfaces;

public interface IPass
{
    IIdentity Enter(IRequest req);
}