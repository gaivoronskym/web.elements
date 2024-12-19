using Web.Elements.Rq;

namespace Web.Elements.Tests.Rq;

public class RqRegexTest
{
    [Fact]
    public void MakesRqRegex()
    {
        var req = new RqRegex(
            new RqFake("GET", "/test/123"),
            "/test/(?<id>\\d+)"
        );
        
        Assert.Equal("123", req.Match().Groups["id"].Value);
    }
}