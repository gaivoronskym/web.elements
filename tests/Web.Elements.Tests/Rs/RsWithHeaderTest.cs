using System.Net;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

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
    
    [Fact]
    public void AddHeaderAsText()
    {
        var response = new RsPrint(
            new RsWithHeader(
                new RsWithStatus(
                    HttpStatusCode.OK
                ),
                new Formatted("{0}: {1}", "test-key", "test-value")
            )
        ).AsString();

        Assert.Contains("test-key", response);
    }
    
    [Fact]
    public void AddHeaderAsString()
    {
        var response = new RsPrint(
            new RsWithHeader(
                new RsWithStatus(
                    HttpStatusCode.OK
                ),
               "test-key: test-value"
            )
        ).AsString();

        Assert.Contains("test-key", response);
    }
}