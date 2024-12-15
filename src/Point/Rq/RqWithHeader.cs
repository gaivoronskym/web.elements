using Point.Rq.Interfaces;

namespace Point.Rq;

public sealed class RqWithHeader : RqWrap
{
    public RqWithHeader(IRequest req, string key, string value)
        : this(
            req,
            $"{key}: {value}"
        )
    {
    }

    public RqWithHeader(IRequest req, string header)
        : base(
            new RqWithHeaders(
                req,
                header
            )
        )
    {
    }
}