using System.Net;
using Point.Rs;

namespace Point.Tests.Rs;

public class RsStatusTest
{
    [Fact]
    public void GetStatusCodeFromResponse()
    {
        var response = new IRsStatus.Base(
            new RsWithStatus(HttpStatusCode.OK)
        );
        
        Assert.Equal(200, response.Status());
    }
}