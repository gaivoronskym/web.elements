using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using Point.Exceptions;
using Point.Http.Token;
using Point.Pt;
using Point.Rq;
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
        while (true)
        {
            await using var networkStream = client.GetStream();
            try
            {
                const int bufferSize = 65536 * 3;
                StreamPipeReaderOptions readerOptions = new(pool: MemoryPool<byte>.Shared, leaveOpen: true, bufferSize: bufferSize);
                var pipe = PipeReader.Create(networkStream, readerOptions);
                var head = await HeaderAsync(pipe);
                var body = await BodyAsync(pipe);

                var res = await point.Act(
                    new RequestOf(
                        head,
                        body
                    )
                );

                new RsPrint(res)
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

    private async Task<IBody> BodyAsync(PipeReader pipe)
    {
        var pipeResult = await pipe.ReadAsync();
        IHttpToken token = new HttpToken(pipe, pipeResult.Buffer);
        token = token.SkipNext(3);

        return new BodyOf(token.Stream);
    }

    private async Task<IHead> HeaderAsync(PipeReader pipe)
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

        return new HeadOf(head);
    }
}