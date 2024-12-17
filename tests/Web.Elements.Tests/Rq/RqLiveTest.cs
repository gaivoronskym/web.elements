using Web.Elements.Rq;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Joined = Yaapii.Atoms.Text.Joined;

namespace Web.Elements.Tests.Rq;

public class RqLiveTest
{
    [Fact]
    public void BuildsHttpRequest()
    {
        var req = new RqMethod(
            new RqLive(
                new InputStreamOf(
                    new Joined(
                        "\r\n",
                        new ManyOf<string>(
                            "POST / HTTP/1.1",
                            "Host: www.example.com",
                            "Content-Length: 5",
                            "TestKey: testValue",
                            "TestKey1: testValue1",
                            "",
                            "hello"
                        )
                    ).AsString()
                )
            )
        );
        
        Assert.Equal("POST", req.Method());
    }

    [Fact]
    public void SupportMultilineHeaders()
    {
        var req = new RqLive(
            new InputStreamOf(
                new Joined(
                    "\r\n",
                    new ManyOf<string>(
                        "GET /multiline HTTP/1.1",
                        "X-Foo: this is a test",
                        " header for you",
                        "",
                        "hello"
                    )
                ).AsString()
            )
        );
        
        Assert.True(new IRqHeaders.Base(req).Header("X-Foo").Contains("this is a test header for you"));
    }

    [Fact]
    public void SupportMultilineHeadersWithColon()
    {
        var req = new RqLive(
            new InputStreamOf(
                new Joined(
                    "\r\n",
                    new ManyOf<string>(
                        "GET /multiline HTTP/1.1",
                        "Foo: first line",
                        " second: line",
                        "",
                        "hello"
                    )
                ).AsString()
            )
        );
        
        Assert.True(new IRqHeaders.Base(req).Header("Foo").Contains("first line second: line"));
    }
    
    [Fact]
    public void FailsOnBrokenHttpRequest()
    {
        Assert.Throws<IOException>(
            () => new RqLive(
                new InputStreamOf(
                    "GET /test HTTP/1.1\r\nHost: \u20ac"
                )
            ).Head()
        );
    }

    [Fact]
    public void FailsOnInvalidNewLineCharacterRequest()
    {
        Assert.Throws<IOException>(
            () => new RqLive(
                new InputStreamOf(
                    "GET /test HTTP/1.1\rHost: localhost"
                )
            ).Head()
        );
    }
}