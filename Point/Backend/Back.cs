using Point.Pt;
using Point.Rq;
using Point.Rs;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using Point.Backend.Http;
using Point.Exceptions;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Backend;

public class Back : IBack
{
    private readonly IPoint point;
    private readonly TcpListener server;

    public Back(IPoint point, int port)
    {
        this.point = point;

        var localAddr = IPAddress.Parse("127.0.0.1");
        this.server = new TcpListener(localAddr, port);
    }

    public async Task StartAsync()
    {
        server.Start();

        while (true)
        {
            var client = await server.AcceptTcpClientAsync();
            var networkStream = client.GetStream();

            try
            {
                const int bufferSize = 65536 * 3;
                StreamPipeReaderOptions readerOptions = new(pool: MemoryPool<byte>.Shared, leaveOpen: true, bufferSize: bufferSize);

                var pipe = PipeReader.Create(networkStream, readerOptions);

                var head = await HeaderAsync(pipe);
                var body = await BodyAsync(pipe);

                Console.WriteLine("-------------Request Begin--------------");

                foreach (var header in head)
                {
                    Console.WriteLine(header);
                }

                Console.WriteLine("-------------Request End--------------");

                var response = await point.Act(
                    new RequestOf(
                        head,
                        body
                    )
                );

                new RsPrint(response)
                    .Print(networkStream);

                client.Close();
            }
            catch (OperationCanceledException)
            {
                if (client.Connected)
                {
                    Console.WriteLine($"Connection to {client.Client.RemoteEndPoint} closed.");
                    networkStream.Close();
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                if (client.Connected)
                {
                    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                    networkStream.Close();
                    client.Close();
                }
            }
        }
    }

    public void Stop()
    {
        server.Stop();
    }

    private async Task<Stream> BodyAsync(PipeReader pipe)
    {
        var pipeResult = await pipe.ReadAsync();
        IHttpToken token = new HttpToken(pipe, pipeResult.Buffer);
        token = token.SkipNext(3);

        return token.Stream();
    }

    private async Task<IList<string>> HeaderAsync(PipeReader pipe)
    {
        var pipeResult = await pipe.ReadAsync();
        IHttpToken token = new HttpToken(pipe, pipeResult.Buffer);

        var firstHead = token.AsString('\r');

        var thisIsNotHttp = new Not(
            new Contains(
                firstHead,
                "HTTP"
            )
        );

        if (thisIsNotHttp.Value())
        {
            throw new HttpException(HttpStatusCode.NotAcceptable);
        }

        string key;

        var head = new List<string> { firstHead };

        token = token.Skip('\r')
            .SkipNext(2);

        while (!string.IsNullOrEmpty(key = token.AsString(':')) && !token.NextIs("\n\r\n"))
        {
            token = token.Skip(':')
          .SkipNext(1);

            string value;

            if (!string.IsNullOrEmpty(value = token.AsString('\r')))
            {
                var header = $"{key.Trim()}: {value.Trim()}";
                head.Add(header);

                token = token.Skip('\r')
                    .SkipNext(1);
            }
        }

        return head;
    }
}