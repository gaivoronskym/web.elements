using Point.Rq.Interfaces;

namespace Point.Pt;

public abstract class PtWrap : IPoint
{
    private readonly IPoint origin;
    private readonly Func<IResponse, IResponse> func;

    public PtWrap(IPoint origin)
        : this(origin, (res) => res)
    {
        
    }
    
    public PtWrap(IPoint origin, Func<IResponse, IResponse> func)
    {
        this.origin = origin;
        this.func = func;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        return func(
            await origin.Act(req)
        );
    }
}