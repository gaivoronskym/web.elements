using Point.Rs;

namespace Point.Pt;

public sealed class PtWithHeader : PtWrap
{
    public PtWithHeader(IPoint origin, string name, string value)
        : this(origin, $"{name}: {value}")
    {
    }

    public PtWithHeader(IPoint origin, string header)
        : base(
            new PointOf(
                async req =>
                {
                    var res = await origin.Act(req);
                    return new RsWithHeader(res, header);
                }
            )
        )
    {
    }
}