﻿using Point.Auth;
using Point.Codec;
using Point.Exceptions;
using Point.Fk;
using Point.Pg;
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