using System.Net;
using Point.Rs;

namespace Point.Tests.Rs;

public class RsWithHeaderTest
{
    [Fact]
    public void AddHeaderToResponse()
    {
        var response = new RsPrint(
            new RsWithHeader(
                new RsWithStatus(
                    HttpStatusCode.OK
                ),
                "test-key",
                "test-value"
            )
        ).AsString();

        Assert.Contains("test-key", response);
    }
}