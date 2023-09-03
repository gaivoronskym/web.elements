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
using Yaapii.Atoms.Text;

namespace Point.Backend;

public class TcpBackend
{
    private readonly TcpListener _server;
    private readonly IPoint _point;

    public TcpBackend(IPoint point, int port)
    {
        _point = point;

        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        _server = new TcpListener(localAddr, port);
    }

    public async Task Start()
    {
        _server.Start();

        // byte[] buffer = new byte[ushort.MaxValue];

        while (true)
        {
            var client = await _server.AcceptTcpClientAsync();
            var networkStream = client.GetStream();

            StreamPipeReaderOptions readerOptions =
                new(pool: MemoryPool<byte>.Shared, leaveOpen: true, bufferSize: 65536);

            try
            {
                var pipe = PipeReader.Create(networkStream, readerOptions);

                var pipeResult = await pipe.ReadAsync();

                IHttpToken token = new HttpToken(pipe, pipeResult.Buffer);
                var method = token.AsString(' ');

                token = token.Skip(' ')
                    .SkipNext(1);

                var path = token.AsString(' ');

                token = token.Skip(' ')
                    .SkipNext(1);

                var version = token.AsString('\r');

                string key;

                /* IDictionary<string, string> header = new Dictionary<string, string>(); */

                ImmutableList<string> head = ImmutableList.Create(
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

                IResponse response = _point.Act(
                    new RequestOf(
                        head,
                        new InputOf(string.Empty).Stream()
                    )
                );
                
                // networkStream.Write(
                //     Encoding.UTF8.GetBytes(
                //         "HTTP/1.0 200 OK" + Environment.NewLine
                //                           + "Content-Length: " + response.Body().Length + Environment.NewLine
                //                           + "Content-Type: " + "text/plain" + Environment.NewLine
                //                           + Environment.NewLine
                //                           + new TextOf(new InputOf(response.Body)).AsString()
                //                           + Environment.NewLine + Environment.NewLine));
                
                foreach (var header in response.Head())
                {
                    if (header.StartsWith("HTTP") || header.StartsWith("Content-Length") || header.StartsWith("Content-Type"))
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

                // var read = await networkStream.ReadAsync(buffer.AsMemory(0, 2)).ConfigureAwait(false);
                // int size = BitConverter.ToUInt16(buffer, 0);
                // int offset = 2;

                // normally there will be only one iteration of this loop but
                // ReadAsync doesn't guarantee that 'received' will always match
                // requested bytes amount
                // while (offset < size && networkStream.DataAvailable)
                // {
                //     int received = await networkStream.ReadAsync(buffer.AsMemory(offset, size - offset))
                //         .ConfigureAwait(false);
                //     if (received == 0)
                //     {
                //         Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected.");
                //         return;
                //     }
                //
                //     offset += received;
                // }
                //
                // var requestRaw = Encoding.UTF8.GetString(buffer.AsSpan(0, offset));
                // Console.WriteLine(requestRaw);

                // var content = new BytesOf(new TextOf("Hello, world")).AsBytes();

                // var fileName = @"F:\VisualStudioProjects\point\CustomServer\bin\Debug\net7.0\file.txt";

                // var file = new FileInfo(fileName);
                // long numBytes = file.Length;

                // var fileArr = await File.ReadAllBytesAsync(file.FullName);

                // StreamWriter writer = new StreamWriter(networkStream);

                // var header = new StringBuilder();
                // header.AppendLine("HTTP/1.0 200 OK");
                // header.AppendLine();
                // header.AppendLine("Content-Type: text/plain; charset=UTF-8");
                // header.AppendLine("Content-Length: " + content.Length);
                // header.AppendLine();
                // header.AppendLine();
                // var result = "<h1>Hello, world!</h1>";
                // networkStream.Write(
                //     Encoding.UTF8.GetBytes(
                //         "HTTP/1.0 200 OK" + Environment.NewLine
                //                           + "Content-Length: " + result.Length + Environment.NewLine
                //                           + "Content-Type: " + "text/plain" + Environment.NewLine
                //                           + Environment.NewLine
                //                           + result
                //                           + Environment.NewLine + Environment.NewLine));

                // await writer.WriteAsync("HTTP/1.1 200 OK");
                // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync(Environment.NewLine);
                
                // foreach (var header in response.Head())
                // {
                //     await writer.WriteAsync(header);
                //     if (header.StartsWith("HTTP"))
                //     {
                //         await writer.WriteAsync(Environment.NewLine);
                //     }
                //
                //     // if (header.StartsWith("Content-Type"))
                //     // {
                //     //     await writer.WriteAsync(Environment.NewLine);
                //     // }
                //     
                //     // if (header.StartsWith("Content-Length"))
                //     // {
                //     //     await writer.WriteAsync(Environment.NewLine);
                //     // }
                // }
                //
                // // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync(Environment.NewLine);
                // // await writer.WriteAsync(new TextOf("Hello, world").AsString());
                //
                // await writer.BaseStream.WriteAsync(
                //     new BytesOf(
                //         new InputOf(
                //             response.Body()
                //         )
                //     ).AsBytes()
                // );
                //
                // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync(Environment.NewLine);
                
                // await writer.WriteAsync("HTTP/1.1 200 OK");
                // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync("Content-Type: text/plain; charset=UTF-8");
                // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync("Content-Length: "+ "Hello, world".Length);
                // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync(Environment.NewLine);
                // await writer.WriteAsync(new TextOf("Hello, world").AsString());
                // byte[] headerBytes = Encoding.UTF8.GetBytes(header.ToString());

                // await writer.BaseStream.WriteAsync(headerBytes, 0, headerBytes.Length);
                // await writer.BaseStream.WriteAsync(content, 0, content.Length);
                // await writer.FlushAsync();

                client.Close();
            }
            catch (OperationCanceledException)
            {
                if (client.Connected)
                {
                    networkStream.Close();
                    Console.WriteLine($"Connection to {client.Client.RemoteEndPoint} closed.");
                }
            }
            catch (Exception ex)
            {
                // Test the class a lot with closing the connection on both sides
                // I'm not sure how it will behave because I didn't test it
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                networkStream.Close();
            }
        }
    }

    public void Stop()
    {
        _server.Stop();
    }
}