using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Point.Rs;

public sealed class RsWithHeader : RsWrap
{
    public RsWithHeader(IResponse origin, string name, string value)
        : this(origin,
            new Formatted("{0}: {1}", name, value).AsString()
        )
    {
    }

    public RsWithHeader(IResponse origin, string header)
        : base(
            new ResponseOf(
                () => new Joined<string>(origin.Head(), header),
                origin.Body
            )
        )
    {
    }
    
    public RsWithHeader(IResponse origin, params string[] headers)
        : base(
            new ResponseOf(
                () => new Joined<string>(origin.Head(), headers),
                origin.Body
            )
        )
    {
    }
}