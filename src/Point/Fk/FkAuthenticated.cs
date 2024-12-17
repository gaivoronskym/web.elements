using System.Net;
using Point.Exceptions;
using Point.Pg;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Fk;

public class FkAuthenticated : IFork
{
    private readonly IScalar<IPage> page;
    private readonly string header;

    public FkAuthenticated(IPage page)
        : this(page, nameof(PgAuth))
    {
    }

    public FkAuthenticated(IPage page, string header)
        : this(new Live<IPage>(() => page), header)
    {
    }

    public FkAuthenticated(IScalar<IPage> page)
        : this(page, nameof(PgAuth))
    {
    }
        
    public FkAuthenticated(IScalar<IPage> page, string header)
    {
        this.page = page;
        this.header = header;
    }

    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        var identity = new RqAuth(req, header).Identity();
        if(!string.IsNullOrEmpty(identity.Identifier()))
        {
            var res = await page.Value().Act(req);
            return new Optinal<IResponse>(res);
        }

        throw new HttpException(HttpStatusCode.Unauthorized);
    }
}