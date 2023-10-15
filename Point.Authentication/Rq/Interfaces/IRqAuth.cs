using Point.Authentication.Interfaces;
using Point.Rq.Interfaces;

namespace Point.Authentication.Rq.Interfaces
{
    public interface IRqAuth : IRequest
    {
        IIdentity Identity();
    }
}
