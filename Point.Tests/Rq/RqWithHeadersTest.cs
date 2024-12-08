using Point.Rq;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Joined = Yaapii.Atoms.Text.Joined;

namespace Point.Tests.Rq;

public class RqWithHeadersTest
{
    [Fact]
    public void AddHeaders()
    {
        var actual =new RqPrint(
            new RqWithHeaders(
                new RqFake(),
                "TestKey: testValue",
                "TestKey1: testValue1"
            )
        ).AsString();
        
        var expected = new Joined(
            "\r\n",
            new ManyOf<string>(
                $"GET / HTTP/1.1",
                "Host: www.example.com",
                "TestKey: testValue",
                "TestKey1: testValue1"
            )
        ).AsString();

        Assert.True(
            new StartsWith(
                new TextOf(actual),
                expected
            ).Value()
        );
    }
}