using Point.Pg;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.Text;

namespace Point.Tests.Pg;

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