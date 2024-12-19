using Web.Elements.Exceptions;
using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Fk;

public class PgForkTest
{
    [Fact]
    public async Task Matches()
    {
        const string body = "Hello";
        var res = await new PgFork(
            new FkRegex(
                "/test",
                new PgText(body)
            )
        ).Act(new RqFake("GET", "/test"));

        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                $"Content-Length: {body.Length}",
                "Content-Type: text/plain",
                "",
                body
            ).AsString(),
            new RsPrint(res).AsString()
        );
    }
    
    [Fact]
    public async Task DoesNotMatch()
    {
        await Assert.ThrowsAsync<HttpException>(
            async () =>
            {
                const string body = "Hello";
                await new PgFork(
                    new FkRegex(
                        "/test",
                        new PgText(body)
                    )
                ).Act(new RqFake("GET", "/users"));
            }
        );
    }
}