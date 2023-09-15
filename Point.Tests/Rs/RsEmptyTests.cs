using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Tests.Rs;

public class RsEmptyTests
{
    [Fact]
    public void MakeNoContentResponse()
    {
        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 204 No Content",
                "",
                ""
            ).AsString(),
            new RsPrint(
                new RsEmpty()
            ).AsString()
        );
    }
}