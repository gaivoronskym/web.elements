using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;

namespace Web.Elements.Tests.Fk;

public class FkChainTest
{
    [Fact]
    public async Task MatchesByRegex()
    {
        var res = await new FkChain(
            new FkRegex(
                "/test",
                new PgText("Hello")
            )
        ).Route(new RqFake("GET", "/test"));
        
        Assert.True(res.Has());
    }
    [Fact]
    public async Task DoesNotMatchByRegex()
    {
        var res = await new FkChain(
            new FkRegex(
                "/users",
                new PgText("Hello")
            )
        ).Route(new RqFake("GET", "/test"));
        
        Assert.False(res.Has());
    }
    
}