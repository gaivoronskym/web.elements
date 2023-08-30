using Point.Rq.Interfaces;

namespace Point.Pt;

public interface IPoint
{
    IResponse Act(IRequest req);
}