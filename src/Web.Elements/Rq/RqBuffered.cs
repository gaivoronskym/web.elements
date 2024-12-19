namespace Web.Elements.Rq;

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