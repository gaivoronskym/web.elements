using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using StringJoined = Yaapii.Atoms.Enumerable.Joined<string>;

namespace Web.Elements.Rs;

public sealed class RsWithHeader : RsWrap
{
    public RsWithHeader(IResponse origin, string name, string value)
        : this(origin,
            new Formatted("{0}: {1}", name, value)
        )
    {
    }

    public RsWithHeader(IResponse origin, string header)
        : this(origin, new TextOf(header))
    {
    }


    public RsWithHeader(IResponse origin, IText header)
        : base(
            new ResponseOf(
                () => new Joined<string>(
                    origin.Head(),
                    new ManyOf<string>(header.AsString())
                ),
                origin.Body
            )
        )
    {
    }
}