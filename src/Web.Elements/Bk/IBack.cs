using System.Net.Sockets;

namespace Web.Elements.Bk;

public interface IBack
{
    Task AcceptAsync(TcpClient client);
}