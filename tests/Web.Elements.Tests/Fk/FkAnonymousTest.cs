using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;

namespace Web.Elements.Tests.Fk;

public class FkAnonymousTest
{
    [Fact]
    public async Task MatchesIfAnonymousUser()
    {
        var opt = await new FkAnonymous(
            new PgEmpty()
        ).Route(
            new RqFake("GET", $"/items/{Guid.NewGuid()}/prices")
        );
        
        Assert.True(opt.Has());
    }
    
}