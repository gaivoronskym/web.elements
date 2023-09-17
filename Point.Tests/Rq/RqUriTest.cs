using Point.Rq;
using Yaapii.Atoms.List;

namespace Point.Tests.Rq;

public class RqUriTest
{
    [Fact]
    public void ParsesHttpQuery()
    {
        Assert.Equal(
                actual: new RqUri(
                    new RqFake(
                        new ListOf<string>(
                            "GET /test?a=5 HTTP/1.1",
                            "Host: www.example.com"
                        ),
                        string.Empty
                    )
                ).Uri().ToString(),
                expected: "http://www.example.com/test?a=5"
            );
    }

    [Fact]
    public void ParsesHttpQueryParams()
    {
        string value = new RqUri(
            new RqFake(
                new ListOf<string>(
                    "GET /test?a=5 HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        ).Query()["a"].ToString()!;
        
        Assert.Equal(
                actual: value,
                expected: "5"
            );
    }

    [Fact]
    public void ParsesRouteParams()
    {
        new RqUri(
            new RqFake(
                new ListOf<string>(
                    "GET /test/ HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        );
    }
}