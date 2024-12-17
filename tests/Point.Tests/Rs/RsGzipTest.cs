using Point.Rs;
using System.IO.Compression;
using System.Text;
using Yaapii.Atoms.Text;

namespace Point.Tests.Rs;

public class RsGzipTest
{
    [Fact]
    public void CompressTextResponse()
    {
        var body = "Hello, world";
        var bytes = Encoding.UTF8.GetBytes(body); 
        var memoryStream = new MemoryStream();

        var gZip = new GZipStream(
            memoryStream,
            CompressionLevel.Optimal
        );

        gZip.Write(bytes, 0, bytes.Length);
        gZip.Close();

        var compressedBytes = memoryStream.ToArray();
        var compressedBody = Encoding.UTF8.GetString(compressedBytes);

        Assert.Equal(
            expected: new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                "Content-Type: text/plain",
                $"Content-Length: {compressedBytes.Length}",
                "Content-Encoding: gzip",
                string.Empty,
                compressedBody
            ).AsString(),
            actual: new RsPrint(
                new RsGzip(
                    new RsText(
                        body
                    ),
                    CompressionLevel.Optimal
                )
            ).AsString()
        );
    }
}