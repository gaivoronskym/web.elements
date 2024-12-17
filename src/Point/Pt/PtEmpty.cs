using Point.Rq;
using Point.Rs;

namespace Point.Pt;

public sealed class PtEmpty : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        return Task.FromResult<IResponse>(new RsEmpty());
    }
}