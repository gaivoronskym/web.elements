using Point.Rq.Interfaces;

namespace Point.Rq;

public sealed class RqBuffered : RqWrap
{
    public RqBuffered(IRequest req)
        : base(
            new RequestOf(
                req.Head,
                () => new BufferedStream(req.Body())
            )
        )
    {
    }
}