using Point.Rq;
using Yaapii.Atoms.List;

namespace Point.Tests.Rq;

public class RqUriTest
{
    private const string RouteParamKey = "Route56321-";
    
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
        var value = new RqUri(
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
        Assert.Equal(
            actual: new RqUri(
                new RqFake(
                    new ListOf<string>(
                        "GET /test/54646 HTTP/1.1",
                        "Host: www.example.com",
                        $"{RouteParamKey}id: 54646"
                    ),
                    string.Empty
                )
            ).Route()["id"],
            expected: "54646"
        );
    }
}