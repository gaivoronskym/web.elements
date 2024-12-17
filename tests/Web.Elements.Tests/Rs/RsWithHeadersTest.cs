using System.Net;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Rs;

public class RsWithHeadersTest
{
    [Fact]
    public void AddHeadersToResponse()
    {
        var response = new RsPrint(
            new RsWithHeaders(
                new RsWithStatus(
                    HttpStatusCode.OK
                ),
                "user_id: 12345",
                "user_name: Test"
            )
        ).AsString();

        Assert.Contains("user_id: 12345", response);
        Assert.Contains("user_name: Test", response);
    }
}