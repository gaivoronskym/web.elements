using Point.Rq.Interfaces;

namespace Point.Branch;

public interface IBranch
{
    IResponse? Route(IRequest req);
}