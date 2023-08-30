using Point.Rq.Interfaces;

namespace Point.Bind;

public interface IBranch
{
    IResponse? Route(IRequest req);
}