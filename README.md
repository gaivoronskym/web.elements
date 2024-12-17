# Web.Elements

C# port of [Java Takes](https://github.com/yegor256/takes).

`new FtAsp(new PgIndex());`

```c#
new FtAsp(
    new PgFork(
        new FkRegex(
            "/books",
            new PgBooks()
        ),
        new FkRegex(
            "/books/(?<id>\\d+)",
            new PgBook()
        )
    )
 )
```


```C#
new FtAsp(
    new PgFork(
        new FkRegex(
            "/books",
            new PgFork(
                new FkMethods(
                    "POST",
                    new PgPostBook()
                ),
                new FkMethods(
                    "GET",
                    new PgBooks()
                )
            )
        )
    )
);
```

```C#
public sealed class PgHome : IPage
{
    public Task<IResponse> Act(IRequest req)
    {
        return Task.FromResult<IResponse>(
            new RsHtml("<h1>Home Page</h1>")
        );
    }
}
```

```C#
new FtAsp(
    new PgAuth(
        new PgFork(
            new FkRegex(
                "/api/login",
                new PgFork(
                    new FkAnonymous(
                        new PgLogin(3600, "<key>")
                    )
                )
            ),
            new FkAuthenticated(
                new PgFork(
                    new FkRegex(
                        "/api/books/(?<id>\\d+)/pages",
                        new PgBookPages()
                    )
                )
            )
        ),
        new PsToken("<key>")
    )
)
```