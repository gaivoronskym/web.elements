using Point.Rq;
using Point.Rs;

namespace Point.Pg;

public interface IPage
{
    Task<IResponse> Act(IRequest req);
}