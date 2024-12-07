using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Point.AspNet;
using Point.Fk;
using Point.Http;
using Point.Pt;

namespace Point.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<KestrelServerOptions>(o =>
            {
                o.AllowSynchronousIO = true;
            });

            var ft = new FtWebHost(
                WebHost.CreateDefaultBuilder(),
                new PtBook()
            );

            await ft.StartAsync(new IExit.Never());

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