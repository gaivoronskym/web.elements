using Point.Fk;
using Point.Http;
using Point.Pt;

namespace Point.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new FtBasic(
                new PtFork(
                    // new FkRegex(
                    //     "^/api/items$",
                    //     new PtPostBook()
                    // ),
                    new FkRegex(
                        "^/api/items/(?<id>\\d+)$",
                        new PtMethods(
                            "GET",
                            new IPtRegex.Fake(
                                new PtBookPages(),
                                "/api/items/(?<id>\\d+)"
                            )
                        )
                    )
                ),
                5000
            ).StartAsync(new IExit.Never());
        }
    }
}