using System.Net.Sockets;

namespace Point.Http;

public interface IBack
{
    Task AcceptAsync(TcpClient client);
}