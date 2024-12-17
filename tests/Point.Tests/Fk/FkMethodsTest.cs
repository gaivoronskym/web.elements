using Point.Fk;
using Point.Pg;
using Point.Rq;

namespace Point.Tests.Fk;

public class FkMethodsTest
{
    [Fact]
    public async Task MatchesByMethod()
    {
        var opt = await new FkMethods(
            "PUT,GET",
            new PgEmpty()
        ).Route(
            new RqFake("GET", "/point")
        );
        
        Assert.True(opt.Has());
    }
    
    [Fact]
    public async Task DoesNotMatchByMethod()
    {
        var opt = await new FkMethods(
            "PUT,GET",
            new PgEmpty()
        ).Route(
            new RqFake("POST", "/point")
        );
        
        Assert.False(opt.Has());
    }
}