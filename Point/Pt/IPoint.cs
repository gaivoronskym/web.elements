using Point.Rq.Interfaces;

namespace Point.Pt;

public interface IPoint
{
    Task<IResponse> Act(IRequest req);
}