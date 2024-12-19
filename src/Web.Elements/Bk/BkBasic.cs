using System.Net;
using System.Net.Sockets;
using Web.Elements.Http;
using Web.Elements.Exceptions;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Bk;

public sealed class BkBasic : IBack
{
    private readonly IPage page;
    
    public BkBasic(IPage page)
    {
        this.page = page;
    }
    
    public async Task AcceptAsync(TcpClient client)
    {
        var networkStream = client.GetStream();
        try
        {
            var res = await page.Act(
                new RqLive(networkStream)
            );

            new RsPrint(res)
                .Print(networkStream);

            client.Close();
        }
        catch (HttpException ex)
        {
            RespondError(
                client,
                networkStream,
                new RsText(
                    new RsWithBody(
                        new RsWithStatus(ex.Code()),
                        $"{ex.Message}\r\n{ex.StackTrace ?? string.Empty}"
                    )
                )
            );
        }
        catch (Exception ex)
        {
            RespondError(
                client,
                networkStream,
                new RsText(
                    new RsWithBody(
                        new RsWithStatus(HttpStatusCode.InternalServerError),
                        $"{ex.Message}\r\n{ex.StackTrace ?? string.Empty}"
                    )
                )
            );
        }
    }

    private void RespondError(TcpClient client, NetworkStream networkStream, IResponse res)
    {
        if (client.Connected)
        {
            new RsPrint(res).Print(networkStream);
            networkStream.Close();
            client.Close();
        }
    }
}