using Point.Pt;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.Text;

public class PtTextTest
{
    [Fact]
    public async Task CreatesTextResponse()
    {
        string body = "Hello, world";
        var response = await new PtText(body).Act(new RqFake());
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