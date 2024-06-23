using Point.Rq.Interfaces;

namespace Point.Fk;

public interface IFork
{
    Task<IOpt<IResponse>> Route(IRequest req);
}