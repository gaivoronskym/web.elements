using System.Buffers;
using System.Collections.Immutable;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Point.Pt;
using Point.Rq;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Backend;

public class TcpBackend : IBackend
{
    private readonly TcpListener _server;
    private readonly IPoint _point;

    public TcpBackend(IPoint point, int port)
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
                StreamPipeReaderOptions readerOptions = new(pool: MemoryPool<byte>.Shared, leaveOpen: true, bufferSize: 65536);

                var pipe = PipeReader.Create(networkStream, readerOptions);
                
                var head = await HeaderAsync(pipe);
                var body = await BodyAsync(pipe);

                IResponse response = _point.Act(
                    new RequestOf(
                        head,
                        body
                    )
                );
                
                foreach (var header in response.Head())
                {
                    var text = new TextOf(header);

                    var expression = new Or(
                        new StartsWith(text, "HTTP"),
                        new StartsWith(text, "Content-Length"),
                        new StartsWith(text, "Content-Type")
                    );
                    
                    if (expression.Value())
                    {
                        networkStream.Write(
                            new BytesOf(
                                new TextOf(header + Environment.NewLine)
                            ).AsBytes()
                        );
                    }
                }
                
                networkStream.Write(
                    new BytesOf(
                        new TextOf(Environment.NewLine)
                    ).AsBytes()
                );

                networkStream.Write(
                    new BytesOf(
                        new InputOf(response.Body)
                    ).AsBytes()
                );

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
        
        var method = token.AsString(' ');
        token = token.Skip(' ')
            .SkipNext(1);
        var path = token.AsString(' ');

        token = token.Skip(' ')
            .SkipNext(1);

        var version = token.AsString('\r');

        var thisIsNotHttp = new Not(
            new StartsWith(
                new TextOf(version),
                "HTTP"
            )
        );

        if (thisIsNotHttp.Value())
        {
            throw new Exception();
        }

        string key;
                
        var head = ImmutableList.Create(
            method,
            path,
            version
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