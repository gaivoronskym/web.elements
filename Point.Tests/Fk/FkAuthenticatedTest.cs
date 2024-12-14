using Point.Authentication;
using Point.Authentication.Codec;
using Point.Authentication.Fk;
using Point.Authentication.Pt;
using Point.Pt;
using Point.Rq;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Point.Tests.Fk;

public class FkAuthenticatedTest
{
    [Fact]
    public async Task MatchesIfAuthenticatedUser()
    {
        var opt = await new FkAuthenticated(
            new PtEmpty()
        ).Route(
            new RqWithHeader(
                new RqFake("GET", "/point"),
                nameof(PtAuth),
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
        var opt = await new FkAuthenticated(
            new PtEmpty()
        ).Route(
            new RqFake("GET", "/point")
        );

        Assert.False(opt.Has());
    }
}