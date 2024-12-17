using Web.Elements.Fk;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Fk;

public class FkTypesTest
{
    [Fact]
    public async Task MatchesByAcceptHeader()
    {
        const string accept = "Accept";
        var opt = await new FkTypes(
            "text/xml",
            new RsEmpty()
        ).Route(
            new RqWithHeader(
                new RqFake(),
                accept,
                "*/*"
            )
        );
        
        Assert.True(opt.Has());
        
        opt = await new FkTypes(
            "application/json",
            new RsEmpty()
        ).Route(
            new RqWithHeader(
                new RqFake(),
                accept,
                "image/*"
            )
        );
        
        Assert.False(opt.Has());
        
        opt = await new FkTypes(
            "*/*",
            new RsEmpty()
        ).Route(
            new RqWithHeader(
                new RqFake(),
                accept,
                "text/html"
            )
        );
        
        Assert.True(opt.Has());
    }

    [Fact]
    public async Task IgnoresWithoutAcceptHeader()
    {
        var opt = await new FkTypes(
            "text/xml",
            new RsEmpty()
        ).Route(
            new RqFake()
        );
        
        Assert.False(opt.Has());
    }

    [Fact]
    public async Task MatchesWithoutAcceptHeader()
    {
        var opt = await new FkTypes(
            "text/xml,*/*",
            new RsEmpty()
        ).Route(
            new RqFake()
        );
        
        Assert.True(opt.Has());
    }
}