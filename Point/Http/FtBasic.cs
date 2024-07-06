using System.Net;
using System.Net.Sockets;
using Point.Pt;

namespace Point.Http;

public sealed class FtBasic : IFront
{
    private readonly IBack back;
    private readonly TcpListener listener;

    public FtBasic(IPoint point)
        : this(point, 80)
    {
    }

    public FtBasic(IPoint point, int port)
         : this(new BkBasic(point), port)
    {
    }

    public FtBasic(IBack back, int port)
        : this(back, new TcpListener(IPAddress.Parse("127.0.0.1"), port))
    {
    }

    public FtBasic(IBack back, TcpListener listener)
    {
        this.back = back;
        this.listener = listener;
    }

    public async Task StartAsync(IExit exit)
    {
        try
        {
            do
            {
                await this.LoopAsync(this.listener);
            } while (!exit.Ready());
        }
        finally
        {
            this.listener.Stop();
        }
    }

    private async Task LoopAsync(TcpListener server)
    {
        try
        {
            var client = await server.AcceptTcpClientAsync();
            await this.back.AcceptAsync(client);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}