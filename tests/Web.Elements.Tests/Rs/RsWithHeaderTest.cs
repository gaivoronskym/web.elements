using System.Net;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Rs;

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