using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Web.Elements.Pg;

public sealed class PgFailure : IPage
{
    private IScalar<Exception> exception;

    public PgFailure(Exception exception)
        : this(() => exception)
    {
    }

    public PgFailure(Func<Exception> func)
        : this(new ScalarOf<Exception>(func))
    {
    }


    public PgFailure(IScalar<Exception> exception)
    {
        this.exception = exception;
    }

    public Task<IResponse> Act(IRequest req)
    {
        throw exception.Value();
    }
}