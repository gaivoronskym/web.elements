using Web.Elements.Rq;

namespace Web.Elements.Tests.Rq;

public sealed class RqWithoutHeaderTest
{
    [Fact]
    public void RemovesHeader()
    {
        var names = new RqHeaders(
            new RqWithoutHeader(
                new RqFake(),
                "Host"
            )
        ).Names();
        
        Assert.DoesNotContain("Host", names);
    }
}