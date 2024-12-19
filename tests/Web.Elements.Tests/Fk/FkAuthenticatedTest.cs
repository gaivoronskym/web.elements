using Web.Elements.Auth;
using Web.Elements.Codec;
using Web.Elements.Exceptions;
using Web.Elements.Fk;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Fk;

public class FkAuthenticatedTest
{
    [Fact]
    public async Task MatchesIfAuthenticatedUser()
    {
        var opt = await new FkAuthenticated(
            new PgEmpty()
        ).Route(
            new RqWithHeader(
                new RqFake("GET", "/point"),
                nameof(PgAuth),
                new TextOf(
                    new BytesOf(
                        new CcPlain().Encode(new IdentityUser(Guid.NewGuid().ToString()))
                    )
                ).AsString()
            )
        );

        Assert.True(opt.Has());
    }
    
    [Fact]
    public async Task DoesNotMatchIfAuthenticatedUser()
    {
        await Assert.ThrowsAsync<HttpException>(async () =>
        {
            await new FkAuthenticated(
                new PgEmpty()
            ).Route(
                new RqFake("GET", "/point")
            );
        });
    }
}