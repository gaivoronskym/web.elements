using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;

namespace Web.Elements.Rs;

public sealed class RsWithHeaders : RsWrap
{
    public RsWithHeaders(IResponse origin, params string[] headers)
        : this(origin, new ManyOf<string>(headers))
    {
    }


    public RsWithHeaders(IResponse origin, params IText[] headers)
        : this(
            origin,
            new ManyOf<IText>(headers)
        )
    {
    }


    public RsWithHeaders(IResponse origin, IEnumerable<IText> headers)
        : this(
            origin,
            new Mapped<IText, string>(
                i => i.AsString(),
                headers
            )
        )
    {
    }


    public RsWithHeaders(IResponse origin, IEnumerable<string> headers)
        : base(
            new ResponseOf(
                () => new Joined<string>(
                    origin.Head(),
                    headers
                ),
                origin.Body
            )
        )
    {
    }
}