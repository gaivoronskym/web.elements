using Point.Rq;
using Point.Rq.Interfaces;

namespace Point.Tests.Rq;

public sealed class RqWithoutHeaderTest
{
    [Fact]
    public void RemovesHeader()
    {
        var names = new IRqHeaders.Base(
            new RqWithoutHeader(
                new RqFake(),
                "Host"
            )
        ).Names();
        
        Assert.DoesNotContain("Host", names);
    }
}