using Point.Rq.Interfaces;

namespace Point.Pt;

public class PtBase : IPoint
{
    private readonly IPoint _origin;

    public PtBase(IPoint origin)
    {
        _origin = origin;
    }

    public Task<IResponse> Act(IRequest req)
    {
        return _origin.Act(req);
    }
}