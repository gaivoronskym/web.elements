using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements;

public interface IPass
{
    IOptional<IIdentity> Enter(IRequest req);

    IResponse Exit(IResponse response, IIdentity identity);
}