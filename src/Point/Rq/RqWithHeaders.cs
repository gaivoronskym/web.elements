using Yaapii.Atoms.List;

namespace Point.Rq;

public sealed class RqWithHeaders : RqWrap
{
    public RqWithHeaders(IRequest origin, params string[] headers)
        : this(origin, new ListOf<string>(headers))
    {
    }

    public RqWithHeaders(IRequest origin, IEnumerable<string> headers)
        : base(
            new RequestOf(
                () => new Joined<string>(
                    new ListOf<string>(
                        origin.Head()
                    ),
                    new ListOf<string>(
                        headers
                    )
                ),
                origin.Body
            )
        )
    {
    }
}