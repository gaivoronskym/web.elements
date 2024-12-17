using Point.Rq;
using Point.Rs;

namespace Point.Fk;

public interface IFork
{
    Task<IOptinal<IResponse>> Route(IRequest req);
}