using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;

namespace Web.Elements.Tests.Fk;

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