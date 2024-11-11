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
                    new FkRegex(
                        "^/api/items$",
                        new PtMethods(
                            "POST",
                            new PtPostBook()
                        )
                    ),
                    new FkRegex(
                        "/api/items/(?<id>\\d+)",
                        new PtMethods(
                            "GET",
                            new PtBookPages()
                        )
                    )
                ),
                5000
            ).StartAsync(new IExit.Never());
        }
    }
}