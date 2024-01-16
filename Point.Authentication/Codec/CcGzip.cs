using Point.Authentication.Interfaces;
using System.IO.Compression;

namespace Point.Authentication.Codec
{
    public class CcGzip : ICodec
    {
        private readonly ICodec _origin;
        private readonly CompressionLevel _compressionLevel;

        public CcGzip(ICodec origin)
            : this(origin, CompressionLevel.Optimal)
        {
        }

        public CcGzip(ICodec origin, CompressionLevel compressionLevel)
        {
            _origin = origin;
            _compressionLevel = compressionLevel;
        }

        public byte[] Encode(IIdentity identity)
        {
            MemoryStream memoryStream = new MemoryStream();
            GZipStream compression = new GZipStream(memoryStream, _compressionLevel);
            compression.Write(_origin.Encode(identity));
            compression.Close();

            return memoryStream.ToArray();
        }

        public IIdentity Decode(byte[] data)
        {
            using MemoryStream memoryStream = new MemoryStream(data);
            GZipStream decompression = new GZipStream(memoryStream, CompressionMode.Decompress);
            using MemoryStream decompressedStream = new MemoryStream();
            decompression.CopyTo(decompressedStream);
            return _origin.Decode(decompressedStream.ToArray());
        }
    }
}
