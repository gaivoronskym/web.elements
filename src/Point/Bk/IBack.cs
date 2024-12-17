using System.Net.Sockets;

namespace Point.Bk;

public interface IBack
{
    Task AcceptAsync(TcpClient client);
}