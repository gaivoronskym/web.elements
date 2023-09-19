using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Pt;

public sealed class PtWithHeader : PtWrap
{
    public PtWithHeader(IPoint origin, string header)
        : base(origin, (res) => new RsWithHeader(res, header))
    {
    }

    public PtWithHeader(IPoint origin, string name, string value)
        : this(origin, $"{name}: {value}")
    {
    }
}