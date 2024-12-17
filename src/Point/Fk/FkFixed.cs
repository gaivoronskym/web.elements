using Point.Pg;
using Point.Rq;
using Point.Rs;

namespace Point.Fk;

public sealed class FkFixed : IFork
{
    private readonly IPage page;

    public FkFixed(IPage page)
    {
        this.page = page;
    }

    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        var res = await page.Act(req);
        return new Optinal<IResponse>(res);
    }
}