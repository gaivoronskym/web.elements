using System.IO.Compression;

namespace Web.Elements.Codec;

public class CcGzip : ICodec
{
    private readonly ICodec origin;
    private readonly CompressionLevel compressionLevel;

    public CcGzip(ICodec origin)
        : this(origin, CompressionLevel.Optimal)
    {
    }

    public CcGzip(ICodec origin, CompressionLevel compressionLevel)
    {
        this.origin = origin;
        this.compressionLevel = compressionLevel;
    }

    public byte[] Encode(IIdentity identity)
    {
        var memoryStream = new MemoryStream();
        var compression = new GZipStream(memoryStream, compressionLevel);
        compression.Write(origin.Encode(identity));
        compression.Close();

        return memoryStream.ToArray();
    }

    public IIdentity Decode(byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        var decompression = new GZipStream(memoryStream, CompressionMode.Decompress);
        using var decompressedStream = new MemoryStream();
        decompression.CopyTo(decompressedStream);
        return origin.Decode(decompressedStream.ToArray());
    }
}