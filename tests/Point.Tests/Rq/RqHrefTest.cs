using Point.Rq;
using Yaapii.Atoms.List;

namespace Point.Tests.Rq;

public class RqHrefTest
{
    
    [Fact]
    public void ParsesHttpQueryParams()
    {
        var value = new IRqHref.Base(
            new RqFake(
                new ListOf<string>(
                    "GET /test?a=5 HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        ).Href().Param("a")[0];

        Assert.Equal(
            actual: value,
            expected: "5"
        );
    }
}