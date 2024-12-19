using System.Net;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rs;

public class RsWithStatusTest
{
    [Fact]
    public void MakeOkResponse()
    {
        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                "",
                ""
            ).AsString(),
            new RsPrint(
                new RsWithStatus(
                    HttpStatusCode.OK
                )
            ).AsString()
        );
    }

    [Fact]
    public void MakeNotFoundResponse()
    {
        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 404 Not Found",
                "",
                ""
            ).AsString(),
            new RsPrint(
                new RsWithStatus(
                    HttpStatusCode.NotFound
                )
            ).AsString()
        );
    }
}