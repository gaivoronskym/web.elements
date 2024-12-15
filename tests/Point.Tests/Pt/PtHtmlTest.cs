using Point.Pt;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Tests.Pt;

public class PtHtmlTest
{
    [Fact]
    public async Task CratesTextResponse()
    {
        string body = "<html><body>Hello World</body></html>";
        var response = await new PtHtml(body).Act(new RqFake());
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