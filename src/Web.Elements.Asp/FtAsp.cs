using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Web.Elements.Exceptions;
using Web.Elements.Ft;
using Web.Elements.Pg;
using Web.Elements.Rs;

namespace Web.Elements.Asp;

public sealed class FtAsp : IFront, IAsyncDisposable
{
    private readonly WebApplication app;
    private readonly IPage page;
    private readonly RequestDelegate requestDelegate;

    public FtAsp(IPage page)
        : this(new string[0], page)
    {
    }
    
    public FtAsp(string[] args, IPage page)
        : this(WebApplication.CreateBuilder(args).Build(), page)
    {
    }

    public FtAsp(WebApplication app, IPage page)
    {
        this.app = app;
        this.page = page;
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
            response = await this.page.Act(new RqOfContext(context.Request));
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

    public ValueTask DisposeAsync()
    {
        return this.app.DisposeAsync();
    }
}