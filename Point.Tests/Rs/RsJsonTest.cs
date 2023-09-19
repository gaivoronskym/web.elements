using System.Text.Json.Nodes;
using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Tests.Rs;

public class RsJsonTest
{
    [Fact]
    public void MakesJsonResponse()
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
                new RsJson(
                    json
                )
            ).AsString()
        );
    }
}