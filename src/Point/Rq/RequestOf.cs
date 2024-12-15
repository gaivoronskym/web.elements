using Point.Rq.Interfaces;

namespace Point.Rq;

public sealed class RequestOf : IRequest
{
    private readonly IHead head;
    private readonly IBody body;

    public RequestOf(Func<IEnumerable<string>> headFunc, Func<Stream> bodyFunc)
        : this(headFunc(), bodyFunc())
    {

    }

    public RequestOf(IHead head, IBody body)
    {
        this.head = head;
        this.body = body;
    }

    public RequestOf(IEnumerable<string> head, Stream body)
        : this(new HeadOf(head), new BodyOf(body))
    {
        
    }
    
    public IEnumerable<string> Head()
    {
        return head.Head();
    }

    public Stream Body()
    {
        return body.Body();
    }
}