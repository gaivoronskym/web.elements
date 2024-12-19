using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public interface IFork
{
    Task<IOptional<IResponse>> Route(IRequest req);
}