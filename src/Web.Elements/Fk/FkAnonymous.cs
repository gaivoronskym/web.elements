using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Web.Elements.Fk;

public sealed class FkAnonymous : IFork
{
    private readonly IScalar<IPage> page;

    public FkAnonymous(IPage page)
        : this(new Live<IPage>(() => page))
    {
    }

    public FkAnonymous(IScalar<IPage> page)
    {
        this.page = page;
    }
    
    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        var identity = new RqAuth(req).Identity();
        if (string.IsNullOrEmpty(identity.Identifier()))
        {
            var res = await page.Value().Act(req);
            return new Optinal<IResponse>(res);
        }

        return new IOptinal<IResponse>.Empty();
    }
}