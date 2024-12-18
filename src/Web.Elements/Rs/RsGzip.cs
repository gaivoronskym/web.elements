using System.IO.Compression;

namespace Web.Elements.Rs;

public class RsGzip : IResponse
{
    private readonly IResponse origin;
    private readonly CompressionLevel compressionLevel;
    private readonly IList<IResponse> zipped;

    public RsGzip(IResponse origin)
        : this(origin, CompressionLevel.Optimal)
    {
    }

    public RsGzip(IResponse origin, CompressionLevel compressionLevel)
    {
        this.origin = origin;
        this.compressionLevel = compressionLevel;
        this.zipped = new List<IResponse>();
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
        if (!zipped.Any())
        {
           var content = Gzip(origin.Body());

           zipped.Add(
               new RsWithHeader(
                   new RsWithBody(
                       new RsWithoutHeader(
                           origin,
                           "Content-Length"
                       ),
                       content
                   ),
                   "Content-Encoding",
                   "gzip"
               )
           );
        }

        return zipped.First();
    }

    private byte[] Gzip(Stream input)
    {
        var memoryStream = new MemoryStream();
        var buffer = new byte[4096];

        var gZipStream = new GZipStream(memoryStream, compressionLevel, true);

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
        gZipStream.Close();

        return memoryStream.ToArray();
    }
}