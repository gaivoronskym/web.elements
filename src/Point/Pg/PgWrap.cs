using Point.Rq;
using Point.Rs;

namespace Point.Pg;

public abstract class PgWrap : IPage
{
    private readonly IPage origin;

    protected PgWrap(IPage origin)
    {
        this.origin = origin;
    }
    
    public Task<IResponse> Act(IRequest req)
    {
        return this.origin.Act(req);
    }
}