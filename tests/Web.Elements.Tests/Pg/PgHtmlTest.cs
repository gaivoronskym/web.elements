using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Pg;

public class PgHtmlTest
{
    [Fact]
    public async Task CratesTextResponse()
    {
        string body = "<html><body>Hello World</body></html>";
        var response = await new PgHtml(body).Act(new RqFake());
        var text = new RsPrint(response).AsString();

        Assert.Equal(
            new Joined(
                "\r\n",
                "HTTP/1.1 200 OK",
                $"Content-Length: {body.Length}",
                "Content-Type: text/html",
                "",
                body
            ).AsString(),
            text
        );
    }
}