using Point.Rq;

namespace Point.Tests.Rq;

public class RqHeadersTest
{
    [Fact]
    public void ParsesHeader()
    {
        string body = "Hello, world";

        Assert.Equal(
            actual: new RqHeaders(
                new RqWithHeaders(
                    new RqFake("POST", "/test HTTP/1.1", body),
                    "Content-Type: text/plain",
                    $"Content-Length: {body.Length}"
                )
            ).Headers()["Content-Type"],
            expected: "text/plain"
        );
    }
}