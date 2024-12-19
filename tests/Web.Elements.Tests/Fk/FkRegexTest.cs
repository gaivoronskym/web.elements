using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Fk;

public class FkRegexTest
{
    [Fact]
    public async Task MatchesByRegularExpression()
    {
        var opt = await new FkRegex(
            "^/items/(?<id>\\d+)/prices$",
            new PgEmpty()
        ).Route(
            new RqFake("GET", "items/12345/prices")
        );
        
        Assert.True(opt.Has());
    }
    
    [Fact]
    public async Task MatchesHomePageByRegularExpression()
    {
        var opt = await new FkRegex(
            "^/items/(?<id>\\d+)/prices$",
            new PgHome()
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
            new PgEmpty()
        ).Route(
            new RqFake("GET", "items/rty4etyet")
        );
        
        Assert.False(opt.Has());
    }
    
    public sealed class PgHome : IPgRegex
    {
        public Task<IResponse> Act(IRqRegex req)
        {
            IResponse response = new RsHtml("<h1>Hello</h1>");
            return Task.FromResult(response);
        }
    }
}