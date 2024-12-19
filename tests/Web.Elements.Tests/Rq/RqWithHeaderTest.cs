using Web.Elements.Rq;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rq;

public class RqWithHeaderTest
{
    [Fact]
    public void PrintsCorrectly()
    {
        var body = "Hello, world";

        Assert.Equal(
            actual: new RqPrint(
                new RqWithHeaders(
                    new RqFake("POST", "/test HTTP/1.1", body),
                    "Content-Type: text/plain",
                    $"Content-Length: {body.Length}"
                )
            ).AsString(),
            expected: new Joined(
                Environment.NewLine,
                "POST /test HTTP/1.1",
                "Host: www.example.com",
                "Content-Type: text/plain",
                "Content-Length: 12\r\n",
                body
            ).AsString()
        );
    }
}