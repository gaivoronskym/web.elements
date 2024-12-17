using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace Point.Sample;

public sealed class PtBookHtml : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        return Task.FromResult<IResponse>(new RsHtml("<h1>Title: Object thinking</h1>"));
    }
}