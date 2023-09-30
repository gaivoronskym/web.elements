using Yaapii.Atoms;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using StringJoined = Yaapii.Atoms.Enumerable.Joined<string>;

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
                () => new StringJoined(origin.Head(), header),
                origin.Body
            )
        )
    {
    }

    public RsWithHeader(IResponse origin, IText header)
        : this(origin, header.AsString())
    {
        
    }

    public RsWithHeader(IResponse origin, params IText[] headers)
        : this(origin, headers.Select(x => x.AsString()).ToArray())
    {
        
    }

    public RsWithHeader(IResponse origin, string header, params IText[] headers)
        : this(
            origin,
            new Joined<string>(
                new ListOf<string>(header),
                headers.Select(x => x.AsString()).ToArray()
            ).ToArray()
        )
    {

    }

    public RsWithHeader(IResponse origin, IHead head)
        : this(origin, head.Head().ToArray())
    {
        
    }

    public RsWithHeader(IResponse origin, params string[] headers)
        : base(
            new ResponseOf(
                () => new StringJoined(origin.Head(), headers),
                origin.Body
            )
        )
    {
    }
}