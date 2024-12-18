using Web.Elements.Rq;

namespace Web.Elements.Tests.Rq;

public class RqEmptyTest
{
    [Fact]
    public void MakesEmptyRequest()
    {
        Assert.Equal(
            actual: new RqPrint(
                new RqEmpty()
            ).AsString(),
            expected: "GET / HTTP/1.1\r\n\r\n"
        );
    }
}