using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using Point.Exceptions;
using Point.Http.Token;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Http;

public sealed class BkBasic : IBack
{
    private readonly IPoint point;
    
    public BkBasic(IPoint point)
    {
        this.point = point;
    }
    
    public async Task AcceptAsync(TcpClient client)
    {
        var networkStream = client.GetStream();
        try
        {
            var res = await point.Act(
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