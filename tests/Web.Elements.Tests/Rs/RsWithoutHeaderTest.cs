using System.Net;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Rs;

public class RsWithoutHeaderTest
{
    [Fact]
    public void RemovesHeaderFromResponse()
    {
        var response = new RsPrint(
            new RsWithoutHeader(
                new RsWithHeader(
                    new RsWithStatus(
                        HttpStatusCode.OK
                    ),
                    "test-key",
                    "test-value"
                ),
                "test-key"
            )
        ).AsString();
        
        Assert.DoesNotContain("test-key", response);
    }
}