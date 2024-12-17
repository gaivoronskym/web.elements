using Point.Rq;
using Point.Rs;

namespace Point.Pt;

public sealed class PointOf : IPoint
{
    private readonly Func<IRequest, Task<IResponse>> src;

    public PointOf(IResponse response)
        : this(_ => Task.FromResult(response))
    {
    }
    
    public PointOf(Func<IRequest, IResponse> src)
        : this(req => Task.FromResult(src(req)))
    {
    }
    
    public PointOf(Func<IRequest, Task<IResponse>> src)
    {
        this.src = src;
    }

    public Task<IResponse> Act(IRequest req)
    {
        return this.src.Invoke(req);
    }
}