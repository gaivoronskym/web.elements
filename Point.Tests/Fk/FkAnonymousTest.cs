using Point.Authentication.Fk;
using Point.Pt;
using Point.Rq;

namespace Point.Tests.Fk;

public class FkAnonymousTest
{
    [Fact]
    public async Task MatchesIfAnonymousUser()
    {
        var opt = await new FkAnonymous(
            new PtEmpty()
        ).Route(
            new RqFake("GET", $"/items/{Guid.NewGuid()}/prices")
        );
        
        Assert.True(opt.Has());
    }
    
}