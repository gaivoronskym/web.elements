using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public sealed class FkFixed : IFork
{
    private readonly IPage page;

    public FkFixed(IPage page)
    {
        this.page = page;
    }

    public async Task<IOptional<IResponse>> Route(IRequest req)
    {
        var res = await page.Act(req);
        return new Optional<IResponse>(res);
    }
}