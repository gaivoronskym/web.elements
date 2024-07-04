using Point.Rq;
using Yaapii.Atoms;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Map;

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
        var value = new RqUri(
            new RqFake(
                new ListOf<string>(
                    "GET /test?a=5 HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        ).Query().AsString("a");

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
                        "GET /api/items/1 HTTP/1.1",
                        "Host: www.example.com"
                    ),
                    string.Empty
                ),
                new ListOf<IKvp>(
                    new KvpOf("id", "1")
                )
            ).Route().AsString("id"),
            expected: "1"
        );
    }
}