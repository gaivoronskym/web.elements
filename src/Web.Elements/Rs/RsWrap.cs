using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Web.Elements.Rs;

public abstract class RsWrap : IResponse
{
    private readonly IScalar<IResponse> origin;


    public RsWrap(IResponse origin)
        : this(() => origin)
    {
    }

    public RsWrap(Func<IResponse> origin)
        : this(new ScalarOf<IResponse>(origin))
    {
    }
    
    public RsWrap(IScalar<IResponse> origin)
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