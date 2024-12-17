using Point.Rs;
using System.IO.Compression;
using System.Text;
using Yaapii.Atoms.Text;

namespace Point.Tests.Rs;

public class RsBrotliTest
{

    [Fact]
    public void CompressTextResponse()
    {
        var body = "Hello, world";
        var bytes = Encoding.UTF8.GetBytes(body);
        var memoryStream = new MemoryStream();

        var brotli = new BrotliStream(
            memoryStream,
            CompressionLevel.Optimal
        );

        brotli.Write(bytes, 0, bytes.Length);
        brotli.Close();

        var compressedBytes = memoryStream.ToArray();
        var compressedBody = Encoding.UTF8.GetString(compressedBytes);

        Assert.Equal(
            expected: new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                "Content-Type: text/plain",
                $"Content-Length: {compressedBytes.Length}",
                "Content-Encoding: br",
                string.Empty,
                compressedBody
            ).AsString(),
            actual: new RsPrint(
                new RsBrotli(
                    new RsText(
                        body
                    ),
                    CompressionLevel.Optimal
                )
            ).AsString()
        );
    }

}