using System.Net;
using System.Text.Json.Nodes;
using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Tests.Rs;

public class RsWithTypeTests
{
    [Fact]
    public void MakeApplicationJsonResponse()
    {
        string json = new JsonObject
        {
            { "Username", "Michael" }
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