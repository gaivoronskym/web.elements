using Point.Rq.Interfaces;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Pt;

public sealed class PtFailure : IPoint
{
    private IScalar<Exception> exception;

    public PtFailure(Exception exception)
        : this(() => exception)
    {
    }

    public PtFailure(Func<Exception> func)
        : this(new ScalarOf<Exception>(func))
    {
    }


    public PtFailure(IScalar<Exception> exception)
    {
        this.exception = exception;
    }

    public Task<IResponse> Act(IRequest req)
    {
        throw exception.Value();
    }
}