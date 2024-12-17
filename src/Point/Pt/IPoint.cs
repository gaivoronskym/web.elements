using Point.Rq;
using Point.Rs;

namespace Point.Pt;

public interface IPoint
{
    Task<IResponse> Act(IRequest req);
}