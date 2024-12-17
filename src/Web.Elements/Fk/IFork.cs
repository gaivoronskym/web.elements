using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public interface IFork
{
    Task<IOptinal<IResponse>> Route(IRequest req);
}