using Point.Fk;
using Point.Pg;
using Point.Rq;

namespace Point.Tests.Fk;

public class FkParamsTest
{
    [Fact]
    public async Task MatchedByRegex()
    {
        var res = await new FkParams(
            "user_id",
            "[0-9]+",
            new PgEmpty()
        ).Route(new RqFake("GET", "/auth?user_id=12345"));
        
        Assert.True(res.Has());
    }
}