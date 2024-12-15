using Point.Rq.Interfaces;
using Yaapii.Atoms.Scalar;

namespace Point.Rq;

public sealed class RequestOf : IRequest
{
    private readonly IHead head;
    private readonly IBody body;

    public RequestOf(Func<IEnumerable<string>> headFunc, Func<Stream> bodyFunc)
        : this(
            new HeadOf(new ScalarOf<IEnumerable<string>>(headFunc)),
            new BodyOf(bodyFunc)
        )
    {
    }

    public RequestOf(IEnumerable<string> head, Stream body)
        : this(new HeadOf(head), new BodyOf(body))
    {
    }

    public RequestOf(IHead head, IBody body)
    {
        this.head = head;
        this.body = body;
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