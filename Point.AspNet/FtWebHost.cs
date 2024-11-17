using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Point.Http;
using Point.Pt;
using Point.Rs;

namespace Point.AspNet;

public sealed class FtWebHost : IFront
{
    private readonly IWebHostBuilder hostBuilder;
    private readonly IPoint point;
    private readonly int port;

    public FtWebHost(IWebHostBuilder hostBuilder, IPoint point)
        : this(hostBuilder, point, 5000)
    {
    }

    public FtWebHost(IWebHostBuilder hostBuilder, IPoint point, int port)
    {
        this.hostBuilder = hostBuilder;
        this.point = point;
        this.port = port;
    }

    public Task StartAsync(IExit exit)
    {
        this.hostBuilder
            .UseKestrel(opt =>
            {
                opt.ListenAnyIP(port);
                opt.AllowSynchronousIO = true;
            })
            .Configure(app =>
            {
                app.Run(ListenAsync);
            })
            .Start();

        while (true)
        {
            if (exit.Ready())
            {
                break;
            }
        }

        return Task.CompletedTask;
    }

    private async Task ListenAsync(HttpContext context)
    {
        try
        {
            var response = await this.point.Act(new RqOfContext(context.Request));
            var rsPrint = new RsContextPrint(response);
            rsPrint.Print(context.Response);
        }
        catch (Exception e)
        {
            var response = new RsWithBody(
                new RsWithStatus(
                    HttpStatusCode.InternalServerError
                ),
                $"{e.Message}\n{e.StackTrace}"
            );

            var rsPrint = new RsContextPrint(response);
            rsPrint.Print(context.Response);
        }
    }
}