using Point.Rq;
using Point.Rs;

namespace Point.Fk;

public interface IFork
{
    Task<IOpt<IResponse>> Route(IRequest req);
}