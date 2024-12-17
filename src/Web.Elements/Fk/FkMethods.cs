using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Enumerable;

namespace Web.Elements.Fk;

public sealed class FkMethods : IFork
{
    private readonly IEnumerable<string> methods;
    private readonly IPage page;
    
    public FkMethods(string method, IPage page)
        : this(new ManyOf<string>(method.Split(',')), page)
    {
    }

    public FkMethods(IEnumerable<string> methods, IPage page)
    {
        this.methods = methods;
        this.page = page;
    }

    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        var method = new RqMethod(req).Method();

        if (methods.Contains(method))
        {
            var res = await this.page.Act(req);
            return new Optinal<IResponse>(res);
        }
        
        return new IOptinal<IResponse>.Empty();
    }
}