using Point.Rq.Interfaces;

namespace Point.Branch;

public interface IBranch
{
    Task<IResponse?> Route(IRequest req);
}