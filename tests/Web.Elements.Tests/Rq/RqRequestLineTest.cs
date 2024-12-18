using Web.Elements.Exceptions;
using Web.Elements.Rq;
using Yaapii.Atoms.List;

namespace Web.Elements.Tests.Rq;

public class RqRequestLineTest
{
    [Fact]
    public void ExtractsMethod()
    {
        var req = new RequestLine(
            new RqFake(
                new ListOf<string>(
                    "GET /test?a=5 HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        );
        
        Assert.Equal("GET", req.Method());
    }
    
    [Fact]
    public void ExtractsUri()
    {
        var req = new RequestLine(
            new RqFake(
                new ListOf<string>(
                    "GET /test?a=5 HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        );
        
        Assert.Equal("/test?a=5", req.Uri());
    }
    
    [Fact]
    public void ExtractsVersion()
    {
        var req = new RequestLine(
            new RqFake(
                new ListOf<string>(
                    "GET /test?a=5 HTTP/1.1",
                    "Host: www.example.com"
                ),
                string.Empty
            )
        );
        
        Assert.Equal("HTTP/1.1", req.Version());
    }

    [Fact]
    public void FailsOnBadRequest()
    {
        var req = new RequestLine(
            new RqFake(
                new ListOf<string>()
            )
        );
        
        Assert.Throws<HttpException>(() => req.Uri());
    }
}