using Point.Rs;

namespace Point.Pt;

public sealed class PtGzip : PtWrap
{
    public PtGzip(IPoint origin) 
        : base(origin, res => new RsGzip(res))
    {
    }
}