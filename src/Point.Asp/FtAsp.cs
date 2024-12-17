using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Point.Exceptions;
using Point.Ft;
using Point.Pt;
using Point.Rs;

namespace Point.Asp;

public sealed class FtAsp : IFront, IAsyncDisposable
{
    private readonly WebApplication app;
    private readonly IPoint point;
    private readonly RequestDelegate requestDelegate;

    public FtAsp(IPoint point)
        : this(new string[0], point)
    {
    }
    
    public FtAsp(string[] args, IPoint point)
        : this(WebApplication.CreateBuilder(args).Build(), point)
    {
    }

    public FtAsp(WebApplication app, IPoint point)
    {
        this.app = app;
        this.point = point;
        this.requestDelegate = RequestDelegate;
    }

    public async Task StartAsync(IExit exit)
    {
        app.Run(this.requestDelegate);

        await this.app.StartAsync();

        while (true)
        {
            if (exit.Ready())
            {
                break;
            }
        }
    }
    
    private async Task RequestDelegate(HttpContext context)
    {
        IResponse response = new RsEmpty();

        try
        {
            response = await this.point.Act(new RqOfContext(context.Request));
        }
        catch (HttpException ex)
        {
            response = new RsWithBody(
                new RsWithStatus(
                    ex.Code()
                ),
                $"{ex.Message}\r\n{ex.StackTrace}"
            );
        }
        catch (Exception ex)
        {
            response = new RsWithBody(
                new RsWithStatus(
                    HttpStatusCode.InternalServerError
                ),
                $"{ex.Message}\r\n{ex.StackTrace}"
            );
        }
        finally
        {
            var rsPrint = new RsContextPrint(response);
            rsPrint.Print(context.Response);
        }
    }

    public void Dispose()
    {
        this.app.DisposeAsync();
    }

    public ValueTask DisposeAsync()
    {
        return this.app.DisposeAsync();
    }
}