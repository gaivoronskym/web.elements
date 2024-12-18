using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Fk;

public class FkFixedTest
{
    [Fact]
    public async Task Match()
    {
        var res = await new FkFixed(
            new PgEmpty()
        ).Route(new RqFake());
        
        Assert.True(res.Has());
        Assert.Equal(
            "HTTP/1.1 204 No Content\r\n\r\n",
            new RsPrint(
                res.Value()
            ).AsString()
        );
    }
}