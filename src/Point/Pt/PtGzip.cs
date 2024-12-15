using Point.Rs;

namespace Point.Pt;

public sealed class PtGzip : PtWrap
{
    public PtGzip(IPoint origin)
        : base(
            new PointOf(
                async req =>
                {
                    var res = await origin.Act(req);
                    return new RsGzip(res);
                }
            )
        )
    {
    }
}