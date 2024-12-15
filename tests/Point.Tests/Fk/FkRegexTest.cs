using Point.Fk;
using Point.Pt;
using Point.Rq;

namespace Point.Tests.Fk;

public class FkRegexTest
{
    [Fact]
    public async Task MatchesByRegularExpression()
    {
        var opt = await new FkRegex(
            "^/items/(?<id>\\d+)/prices$",
            new PtEmpty()
        ).Route(
            new RqFake("GET", "items/12345/prices")
        );
        
        Assert.True(opt.Has());
    }

    [Fact]
    public async Task DoesNotMatchByRegularExpression()
    {
        var opt = await new FkRegex(
            "^items/(?<id>\\d+)/prices$",
            new PtEmpty()
        ).Route(
            new RqFake("GET", "items/rty4etyet")
        );
        
        Assert.False(opt.Has());
    }
}