using System.Text.Json.Nodes;
using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Tests.Rs;

public class RsJsonTests
{
    [Fact]
    public void MakesJsonResponse()
    {
        JsonNode node = new JsonObject
        {
            { "Username", "Michael" }
        };

        string json = node.ToJsonString();
        
        Assert.Equal(
            new RsPrint(
                new RsJson(
                    json
                )
            ).AsString(),
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                $"Content-Length: {json.Length}",
                "Content-Type: application/json",
                "",
                json
            ).AsString()
        );
    }
}