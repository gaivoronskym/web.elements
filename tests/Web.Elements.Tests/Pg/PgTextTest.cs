using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Pg;

public class PgTextTest
{
    [Fact]
    public async Task CreatesTextResponse()
    {
        string body = "Hello, world";
        var response = await new PgText(body).Act(new RqFake());
        var text = new RsPrint(response).AsString();

        Assert.Equal(
            new Joined(
                "\r\n",
                "HTTP/1.1 200 OK",
                $"Content-Length: {body.Length}",
                "Content-Type: text/plain",
                "",
                body
            ).AsString(),
            text
        );
    }
}