using System.Net;
using Web.Elements.Rs;

namespace Web.Elements.Tests.Rs;

public class RsStatusTest
{
    [Fact]
    public void GetStatusCodeFromResponse()
    {
        var response = new RsStatus(
            new RsWithStatus(HttpStatusCode.OK)
        );
        
        Assert.Equal(200, response.Status());
    }
}