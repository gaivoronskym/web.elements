using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.RestDoc.Pt;

public sealed class PtDoc : IPoint
{
    private readonly ISegment _segment;

    public PtDoc(ISegment segment)
    {
        _segment = segment;
    }

    public Task<IResponse> Act(IRequest req)
    {
        var rs = new RsJson(
            _segment.Doc().Content()
        );
        
        return Task.FromResult<IResponse>(rs);
    }
}