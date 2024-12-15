using Point.Rs;

namespace Point.Tests.Rs;

public class RsRedirectTest
{
    [Fact]
    public void Redirects()
    {
        var response = new RsPrint(
            new RsRedirect(
                "/home"
            )
        ).AsString();

        Assert.Contains("See Other", response);
    }
}