using Point.Fk;
using Point.Http;
using Point.Ps;
using Point.Pt;

namespace Point.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new FtBasic(
                new PtAuth(
                    new PtFork(
                        new FkRegex(
                            "^/api/login$",
                            new PtFork(
                                new FkAnonymous(
                                    new PtLogin(3600, "FG43553YH343G34353453")
                                )
                            )
                        ),
                        new FkAuthenticated(
                            new PtFork(
                                new FkRegex(
                                    "^/api/items/(?<id>\\d+)$",
                                    new PtBookPages()
                                )
                            )
                        )
                    ),
                    new PsToken("FG43553YH343G34353453")
                ),
                5000
            ).StartAsync(new IExit.Never());
        }
    }
}