using Point.Rq;
using Point.Rq.Interfaces;
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
            actual: new IRqUri.Base(
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
        var value = new IRqUri.Base(
            new RqFake(
                new ListOf<string>(
                    "GET /test?a=5 HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        ).Query()["a"];

        Assert.Equal(
            actual: value,
            expected: "5"
        );
    }
}