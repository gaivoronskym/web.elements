using Point.Rq;
using Point.Rs;

namespace Point.Pt;

public abstract class PtWrap : IPoint
{
    private readonly IPoint origin;

    protected PtWrap(IPoint origin)
    {
        this.origin = origin;
    }
    
    public Task<IResponse> Act(IRequest req)
    {
        return this.origin.Act(req);
    }
}