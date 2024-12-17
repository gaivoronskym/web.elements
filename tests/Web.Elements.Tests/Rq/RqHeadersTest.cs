using Web.Elements.Rq;
using Yaapii.Atoms.Enumerable;

namespace Web.Elements.Tests.Rq;

public class RqHeadersTest
{
    [Fact]
    public void ParsesContentType()
    {
        var body = "Hello, world";

        var list = new IRqHeaders.Base(
            new RqWithHeaders(
                new RqFake(
                    "POST",
                    "/test HTTP/1.1",
                    body
                ),
                "Content-Type: text/plain",
                $"Content-Length: {body.Length}"
            )
        ).Header("Content-Type");
        
        Assert.Contains("text/plain", list);
    }
    
    [Fact]
    public void ParsesHost()
    {
        var list = new IRqHeaders.Base(
            new RqFake(
                new ManyOf<string>(
                    "GET /api/items HTTP/1.1",
                    "Host: example.com",
                    "Content-Type: text/plain"
                )
            )
        ).Header("Host");
        
        Assert.Contains("example.com", list);
    }
    
    [Fact]
    public void FindAll()
    {
        Assert.Equal(
            actual: new IRqHeaders.Base(
                new RqFake(
                    new ManyOf<string>(
                        "GET /api/items HTTP/1.1",
                        "Host: example.com",
                        "Accept: text/xml",
                        "Accept: text/html",
                        "Content-Type: text/plain"
                    )
                )
            ).Header("Accept").Count(),
            expected: 2
        );
    }
}