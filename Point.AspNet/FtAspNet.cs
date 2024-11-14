using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Point.Http;
using Point.Pt;
using Point.Rs;

namespace Point.AspNet;

public sealed class FtAspNet : IFront
{
    private readonly WebApplicationBuilder appBuilder;
    private readonly IPoint point;
    private readonly RequestDelegate requestDelegate;

    public FtAspNet(string[] args, IPoint point)
        : this(WebApplication.CreateBuilder(args), point)
    {
    }

    public FtAspNet(WebApplicationBuilder appBuilder, IPoint point)
    {
        this.appBuilder = appBuilder;
        this.point = point;
        this.requestDelegate = async context =>
        {
            try
            {
                var response = await this.point.Act(new RqAspNet(context.Request));
                var rsPrint = new RsAspPrint(response);
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

                var rsPrint = new RsAspPrint(response);
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