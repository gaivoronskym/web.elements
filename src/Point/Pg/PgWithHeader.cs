using Point.Rs;

namespace Point.Pg;

public sealed class PgWithHeader : PgWrap
{
    public PgWithHeader(IPage origin, string name, string value)
        : this(origin, $"{name}: {value}")
    {
    }

    public PgWithHeader(IPage origin, string header)
        : base(
            new PageOf(
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