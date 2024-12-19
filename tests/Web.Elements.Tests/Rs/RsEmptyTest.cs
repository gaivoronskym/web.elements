using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rs;

public class RsEmptyTest
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