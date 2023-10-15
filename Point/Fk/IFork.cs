using Point.Rq.Interfaces;

namespace Point.Fk;

public interface IFork
{
    Task<IResponse?> Route(IRequest req);
}