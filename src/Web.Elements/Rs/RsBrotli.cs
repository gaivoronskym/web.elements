using System.IO.Compression;

namespace Web.Elements.Rs;

public class RsBrotli : IResponse
{
    private readonly IResponse origin;
    private readonly CompressionLevel compressionLevel;
    private readonly IList<IResponse> compressed;

    public RsBrotli(IResponse origin)
        : this(origin, CompressionLevel.Optimal)
    {
    }

    public RsBrotli(IResponse origin, CompressionLevel compressionLevel)
    {
        this.origin = origin;
        this.compressionLevel = compressionLevel;
        this.compressed = new List<IResponse>();
    }

    public IEnumerable<string> Head()
    {
        return Make().Head();
    }

    public Stream Body()
    {
        return Make().Body();
    }

    private IResponse Make()
    {
        if (!compressed.Any())
        {
            var content = Brotli(origin.Body());

            compressed.Add(
                new RsWithHeader(
                    new RsWithBody(
                        new RsWithoutHeader(
                            origin,
                            "Content-Length"
                        ),
                        content
                    ),
                    "Content-Encoding",
                    "br"
                )
            );
        }

        return compressed.First();
    }

    private byte[] Brotli(Stream input)
    {
        var memoryStream = new MemoryStream();
        var buffer = new byte[4096];

        using var brotliStream = new BrotliStream(memoryStream, compressionLevel, true);

        while (true)
        {
            var len = input.Read(buffer);

            if (len <= 0)
            {
                break;
            }

            brotliStream.Write(buffer, 0, len);
        }

        input.Close();
        //brotliStream.Flush();
        brotliStream.Close();

        return memoryStream.ToArray();
    }
}