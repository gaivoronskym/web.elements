using System.Net;
using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public class RqMultipart : IRqMultipart
{
    private readonly IRequest _origin;
    
    public RqMultipart(IRequest origin)
    {
        _origin = origin;
    }

    public IEnumerable<IRequest> Part(string name)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> Names()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> Head()
    {
        return _origin.Head();
    }

    public Stream Body()
    {
        return _origin.Body();
    }

    private IDictionary<string, IList<IRequest>> Requests(IRequest req)
    {
        string header = new RqHeaders(req).Headers()["Content-Type"];
        IScalar<bool> multipart = new StartsWith(
            new Lower(new TextOf(header)),
            new Lower(new TextOf("multipart/form-data"))
        );

        if (!multipart.Value())
        {
            throw new HttpRequestException(
                "RqMultipart can can only parse multipart/form-data",
                null,
                HttpStatusCode.BadRequest
            );
        }

        StreamReader reader = new StreamReader(Body());
        
        IList<IRequest> requests = new List<IRequest>();

        var boundary = new BytesOf(
            reader.ReadToEnd()
        ).AsBytes();

        
        while (!reader.EndOfStream)
        {
            
        }
    }

    private IRequest Make(Stream stream, IEnumerable<byte> boundary)
    {
        //todo
    }

    private static IDictionary<string, IList<IRequest>> AsMap(IList<IRequest> reqs)
    {
        //todo
        return new Dictionary<string, IList<IRequest>>();
    }
}