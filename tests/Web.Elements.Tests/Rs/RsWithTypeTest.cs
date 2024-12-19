using System.Net;
using System.Text.Json.Nodes;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rs;

public class RsWithTypeTest
{
    [Fact]
    public void MakeApplicationJsonResponse()
    {
        var json = new JsonObject
        {
            { "Username", "Test" }
        }.ToJsonString();

        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                $"Content-Length: {json.Length}",
                "Content-Type: application/json",
                "",
                json
            ).AsString(),
            new RsPrint(
                new RsWithStatus(
                    new RsWithType(
                        new RsWithBody(json),
                        "application/json"
                    ),
                    HttpStatusCode.OK
                )
            ).AsString()
        );
    }
}