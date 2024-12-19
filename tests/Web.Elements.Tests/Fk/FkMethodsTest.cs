using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;

namespace Web.Elements.Tests.Fk;

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