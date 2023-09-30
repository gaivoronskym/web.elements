using System.IO.Compression;
using System.Text;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;

namespace Point.Rs;

public class RsGzip : IResponse
{
    private readonly IResponse _origin;
    private readonly CompressionLevel _compressionLevel;
    private readonly IList<IResponse> _zipped;

    public RsGzip(IResponse origin)
        : this(origin, CompressionLevel.Optimal)
    {
    }

    public RsGzip(IResponse origin, CompressionLevel compressionLevel)
    {
        _origin = origin;
        _compressionLevel = compressionLevel;
        _zipped = new List<IResponse>();
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
        if (!_zipped.Any())
        {
           byte[] zipped = Gzip(_origin.Body());

            _zipped.Add(
                new RsWithHeader(
                    new RsWithBody(
                        new RsWithoutHeader(
                            _origin,
                            "Content-Length"
                        ),
                        zipped
                    ),
                    "Content-Encoding",
                    "gzip"
                )
            );
        }

        return _zipped.First();
    }

    private byte[] Gzip(Stream input)
    {
        MemoryStream memoryStream = new MemoryStream();
        byte[] buffer = new byte[4096];

        GZipStream gZipStream = new GZipStream(memoryStream, _compressionLevel, true);

        while (true)
        {
            var len = input.Read(buffer);

            if (len <= 0)
            {
                break;
            }
            
            gZipStream.Write(buffer, 0, len);
        }
        
        input.Close();
        //gZipStream.Flush();
        gZipStream.Close();

        return memoryStream.ToArray();
    }
}