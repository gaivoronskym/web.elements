using Point.Exceptions;
using Point.Rq;
using Yaapii.Atoms.List;

namespace Point.Tests.Rq;

public class RqRequestLineTest
{
    [Fact]
    public void ExtractsMethod()
    {
        var req = new IRqRequestLine.Base(
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
        var req = new IRqRequestLine.Base(
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
        var req = new IRqRequestLine.Base(
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
        var req = new IRqRequestLine.Base(
            new RqFake(
                new ListOf<string>()
            )
        );
        
        Assert.Throws<HttpException>(() => req.Uri());
    }
}