using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Web.Elements.Rq;

public abstract class RqWrap : IRequest
{
    private readonly IScalar<IRequest> origin;

    public RqWrap(IRequest origin)
        : this(new ScalarOf<IRequest>(() => origin))
    {
    }
    
    public RqWrap(IScalar<IRequest> origin)
    {
        this.origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return origin.Value().Head();
    }

    public Stream Body()
    {
        return origin.Value().Body();
    }
}