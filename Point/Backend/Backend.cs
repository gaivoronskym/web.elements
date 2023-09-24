using System.Buffers;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using Point.Pt;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Backend;

public class Backend : IBackend
{
    private readonly TcpListener _server;
    private readonly IPoint _point;

    public Backend(IPoint point, int port)
    {
        _point = point;

        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        _server = new TcpListener(localAddr, port);
    }

    public async Task StartAsync()
    {
        _server.Start();
        
        while (true)
        {
            var client = await _server.AcceptTcpClientAsync();
            var networkStream = client.GetStream();

            try
            {
                var bufferSize = 65536 * 3;
                StreamPipeReaderOptions readerOptions = new(pool: MemoryPool<byte>.Shared, leaveOpen: true, bufferSize: bufferSize);

                var pipe = PipeReader.Create(networkStream, readerOptions);
                
                var head = await HeaderAsync(pipe);
                var body = await BodyAsync(pipe);

                Console.WriteLine("-------------Request--------------");

                foreach(var header in head)
                {
                    Console.WriteLine(header);
                }

                Console.WriteLine("-------------Request--------------");

                IResponse response = await _point.Act(
                    new RequestOf(
                        head,
                        body
                    )
                );

                //Debug.WriteLine(
                //    new RsPrint(response)
                //    .Print()
                //);

                //var temp = new RsPrint(response)
                //    .Print();

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
        _server.Stop();
    }

    private async Task<Stream> BodyAsync(PipeReader pipe)
    {
        var pipeResult = await pipe.ReadAsync();
        IHttpToken token = new HttpToken(pipe, pipeResult.Buffer);
        token = token.SkipNext(3);

        return token.Stream();
    }
    
    private async Task<ImmutableList<string>> HeaderAsync(PipeReader pipe)
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
            throw new Exception();
        }

        string key;
                
        var head = ImmutableList.Create(
            firstHead
        );

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
                head = head.Add(header);

                token = token.Skip('\r')
                    .SkipNext(1);
            }
        }

        return head;
    }
}