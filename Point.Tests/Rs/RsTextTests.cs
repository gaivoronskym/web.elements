using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Tests.Rs;

public class RsTextTests
{
    [Fact]
    public void MakesTextPlainResponse()
    {
        string body = "Hello, world";

        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                $"Content-Length: {body.Length}",
                "Content-Type: text/plain",
                "",
                body
            ).AsString(),
            new RsPrint(
                new RsText(
                    body
                )
            ).AsString()
        );
    }
}