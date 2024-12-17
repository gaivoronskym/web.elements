using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace Point.Fk;

public sealed class FkFixed : IFork
{
    private readonly IPoint point;

    public FkFixed(IPoint point)
    {
        this.point = point;
    }

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        var res = await point.Act(req);
        return new Opt<IResponse>(res);
    }
}