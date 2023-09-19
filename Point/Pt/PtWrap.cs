using Point.Rq.Interfaces;

namespace Point.Pt;

public abstract class PtWrap : IPoint
{
    private readonly IPoint _origin;
    private readonly Func<IResponse, IResponse> _func;

    public PtWrap(IPoint origin)
        : this(origin, (res) => res)
    {
        
    }
    
    public PtWrap(IPoint origin, Func<IResponse, IResponse> func)
    {
        _origin = origin;
        _func = func;
    }

    public IResponse Act(IRequest req)
    {
        return _func(
            _origin.Act(req)
        );
    }
}