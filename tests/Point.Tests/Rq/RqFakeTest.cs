using System.Text.Json.Nodes;
using Point.Rq;

namespace Point.Tests.Rq;

public class RqFakeTest
{
    [Fact]
    public void PrintsCorrectly()
    {
        var json = new JsonObject
        {
            { "name", "Jeff" }
        }.ToJsonString();

        Assert.Equal(
            actual: new RqPrint(
                new RqFake(
                    method: "POST",
                    query: "/test HTTP/1.1",
                    body: json
                )
            ).AsString(),
            expected: $"POST /test HTTP/1.1\r\nHost: www.example.com\r\n\r\n{json}"
        );
    }
}