using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Pg;

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