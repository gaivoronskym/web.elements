using Web.Elements.Rq;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rq;

public class RqWithBodyTest
{
    [Fact]
    public void ReturnsBody()
    {
        var body = "Some body";

        var rqBody = new RqWithHeader(
            new RqWithHeader(
                new RqWithBody(
                    new RqFake(),
                    body
                ),
                "Content-Type",
                "text/plain"
            ),
            "Content-Length",
            $"{body.Length}"
        ).Body();

        var textBody = new TextOf(
            new InputOf(
                rqBody
            )
        ).AsString();
        
        Assert.Equal(body, textBody);
    }
}