using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Pg;

public sealed class PgHtml : PgWrap
{
    public PgHtml(string body)
        : this(new Live<string>(() => body))
    {
    }
    
    public PgHtml(IScalar<string> body)
        : base(
            new PageOf(
                req => new RsHtml(body.Value())
            )
        )
    {
    }
}