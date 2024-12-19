using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

public sealed class RqWithBody : RqWrap
{
    public RqWithBody(IRequest req, string body)
        : this(req, new BytesOf(new TextOf(body)))
    {
    }
    
    public RqWithBody(IRequest req, byte[] bytes)
        : this(req, new BytesOf(bytes))
    {
    }
    
    public RqWithBody(IRequest req, IBytes bytes)
        : this(req, new InputOf(bytes))
    {
    }
    
    public RqWithBody(IRequest req, IInput input)
        : base(
            new RequestOf(
                req.Head,
                input.Stream
            )
        )
    {
    }
}