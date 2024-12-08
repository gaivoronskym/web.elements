using Point.Rq;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Joined = Yaapii.Atoms.Text.Joined;

namespace Point.Tests.Rq;

public class RqLiveTest
{
    [Fact]
    public void BuildsHttpRequest()
    {
        var reqText = new Joined(
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
        ).AsString();

        var req = new RqMethod(
            new RqLive(
                new InputStreamOf(
                    reqText
                )
            )
        );
        
        Assert.Equal("POST", req.Method());
    }
}