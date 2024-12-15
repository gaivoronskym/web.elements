using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Sample;

public sealed class PtLogin : IPoint
{
    public PtLogin()
    {
    }

    public Task<IResponse> Act(IRequest req)
    {
        return this.FromTask(new RsEmpty());
    }

    private Task<IResponse> FromTask(IResponse res)
    {
        return Task.FromResult(res);
    }
}