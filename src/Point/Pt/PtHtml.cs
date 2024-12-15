using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Pt;

public sealed class PtHtml : PtWrap
{
    public PtHtml(string body)
        : this(new Live<string>(() => body))
    {
    }
    
    public PtHtml(IScalar<string> body)
        : base(
            new PointOf(
                req => new RsHtml(body.Value())
            )
        )
    {
    }
}