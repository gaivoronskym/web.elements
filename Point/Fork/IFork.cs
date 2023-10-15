using Point.Rq.Interfaces;

namespace Point.Fork;

public interface IFork
{
    Task<IResponse?> Route(IRequest req);
}