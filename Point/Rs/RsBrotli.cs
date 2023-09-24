using System.IO.Compression;
using System.Text;

namespace Point.Rs
{
    public class RsBrotli : IResponse
    {
        private readonly IResponse _origin;
        private readonly CompressionLevel _compressionLevel;
        private readonly IList<IResponse> _compressed;

        public RsBrotli(IResponse origin)
                : this(origin, CompressionLevel.Optimal)
        {
        }

        public RsBrotli(IResponse origin, CompressionLevel compressionLevel)
        {
            _origin = origin;
            _compressionLevel = compressionLevel;
            _compressed = new List<IResponse>();
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
            if (!_compressed.Any())
            {
                var compressed = Brotli(_origin.Body());
                var base64 = Convert.ToBase64String(compressed);
                var base64Bytes = Encoding.UTF8.GetBytes(base64);
                _compressed.Add(
                    new RsWithHeader(
                        new RsWithBody(
                            new RsWithoutHeader(
                                _origin,
                                "Content-Length"
                            ),
                            compressed
                        ),
                        "Content-Encoding",
                        "br"
                    )
                );
            }

            return _compressed.First();
        }

        private byte[] Brotli(Stream input)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[4096];

            using BrotliStream brotliStream = new BrotliStream(memoryStream, _compressionLevel, true);

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
            brotliStream.Flush();

            return memoryStream.ToArray();
        }
    }
}
