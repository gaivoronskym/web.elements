using Point.Fk;
using Point.Pg;
using Point.Rq;

namespace Point.Tests.Fk;

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