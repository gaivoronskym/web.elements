using System.Net;
using Point.Exceptions;
using Point.Fk;
using Yaapii.Atoms.List;

namespace Point.Pg;

public sealed class PgMethods : PgWrap
{
    public PgMethods(string method, IPage origin)
        : this(new ListOf<string>(method), origin)
    {
    }

    public PgMethods(IEnumerable<string> methods, IPage origin)
        : base(
            new PgFork(
                new FkMethods(
                    methods,
                    origin
                ),
                new FkFixed(
                    new PgFailure(
                        new HttpException(HttpStatusCode.BadRequest)
                    )
                )
            )
        )
    {
    }
}