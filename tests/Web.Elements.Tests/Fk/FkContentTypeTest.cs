using Web.Elements.Fk;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Fk;

public class FkContentTypeTest
{
    [Fact]
    public async Task MatchesWithAnyTypes()
    {
        var opt = await new FkContentType("text/xml", new RsEmpty())
            .Route(
                new RqWithHeader(
                    new RqFake(),
                    "Content-Type",
                    "*/*"
                )
            );
        
        Assert.True(opt.Has());
    }

    [Fact]
    public async Task MatchesDifferentTypes()
    {
        var opt = await new FkContentType("application/json charset=utf-8", new RsEmpty())
            .Route(
                new RqWithHeader(
                    new RqFake(),
                    "Content-Type",
                    "images/*"
                )
            );
        
        Assert.False(opt.Has());
    }

    [Fact]
    public async Task MatchesDifferentEncodingTypes()
    {
        var opt = await new FkContentType("text/html charset=iso-8859-1", new RsEmpty())
            .Route(
                new RqWithHeader(
                    new RqFake(),
                    "Content-Type",
                    "text/html charset=utf-8"
                )
            );
        
        Assert.False(opt.Has());
    }
}