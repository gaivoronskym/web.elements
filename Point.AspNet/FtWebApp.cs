using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Point.Http;
using Point.Pt;
using Point.Rs;

namespace Point.AspNet;

public sealed class FtWebApp : IFront
{
    private readonly WebApplicationBuilder appBuilder;
    private readonly IPoint point;
    private readonly RequestDelegate requestDelegate;

    public FtWebApp(string[] args, IPoint point)
        : this(WebApplication.CreateBuilder(args), point)
    {
    }

    public FtWebApp(WebApplicationBuilder appBuilder, IPoint point)
    {
        this.appBuilder = appBuilder;
        this.point = point;
        this.requestDelegate = async context =>
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
        };
    }

    public async Task StartAsync(IExit exit)
    {
        var app = this.appBuilder.Build();

        app.Run(this.requestDelegate);

        await app.StartAsync();

        while (true)
        {
            if (exit.Ready())
            {
                break;
            }
        }
    }
}