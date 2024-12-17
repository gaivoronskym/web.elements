using Web.Elements.Rs;

namespace Web.Elements.Pg;

public sealed class PgGzip : PgWrap
{
    public PgGzip(IPage origin)
        : base(
            new PageOf(
                async req =>
                {
                    var res = await origin.Act(req);
                    return new RsGzip(res);
                }
            )
        )
    {
    }
}