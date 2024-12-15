using Point.Fk;
using Point.Rq;
using Point.Rs;

namespace Point.Tests.Fk;

public class FkEncodingTest
{
    [Fact]
    public async Task MatchedByAcceptEncodingHeader()
    {
        const string header = "Accept-Encoding";

        var opt = await new FkEncoding("gzip", new RsEmpty()).Route(
            new RqWithHeader(
                new RqFake(),
                header,
                "gzip,exi"
            )
        );
        
        Assert.True(opt.Has());
        
        opt = await new FkEncoding("deflate", new RsEmpty()).Route(
            new RqWithHeader(
                new RqFake(),
                header,
                "gzip,exi"
            )
        );
        
        Assert.False(opt.Has());
    }
}