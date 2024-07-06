using System.Net;
using Point.Exceptions;
using Point.Fk;
using Yaapii.Atoms.List;

namespace Point.Pt;

public sealed class PtMethods : PtWrap
{
    public PtMethods(string method, IPoint origin)
        : this(new ListOf<string>(method), origin)
    {
    }

    public PtMethods(IList<string> methods, IPoint origin)
        : base(
            new PtFork(
                new FkMethods(
                    methods,
                    origin
                ),
                new FkFixed(
                    new PtFailure(
                        new HttpException(HttpStatusCode.BadRequest)
                    )
                )
            )
        )
    {
    }
}