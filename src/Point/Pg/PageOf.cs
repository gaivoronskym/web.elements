using Point.Rq;
using Point.Rs;

namespace Point.Pg;

public sealed class PageOf : IPage
{
    private readonly Func<IRequest, Task<IResponse>> src;

    public PageOf(IResponse response)
        : this(_ => Task.FromResult(response))
    {
    }
    
    public PageOf(Func<IRequest, IResponse> src)
        : this(req => Task.FromResult(src(req)))
    {
    }
    
    public PageOf(Func<IRequest, Task<IResponse>> src)
    {
        this.src = src;
    }

    public Task<IResponse> Act(IRequest req)
    {
        return this.src.Invoke(req);
    }
}