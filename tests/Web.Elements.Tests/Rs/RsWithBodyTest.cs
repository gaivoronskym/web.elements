using System.Net;
using Web.Elements.Rs;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rs;

public class RsWithBodyTest
{
    [Fact]
    public void MakesRsWithTextBody()
    {
        var body = "Hello, world";

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
                new RsWithHeaders(
                    new RsWithBody(
                        new RsWithStatus(HttpStatusCode.OK),
                        body
                    ),
                    "Content-Type: text/plain"
                )
            ).AsString()
        );
    }
    
    [Fact]
    public void MakesRsWithTextBodyFromInputStream()
    {
        var body = "Response Text";

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
                new RsWithHeaders(
                    new RsWithBody(
                        new InputStreamOf(body)
                    ),
                    "Content-Type: text/plain"
                )
            ).AsString()
        );
    }
}