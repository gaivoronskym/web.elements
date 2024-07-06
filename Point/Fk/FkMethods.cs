using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;

namespace Point.Fk;

public sealed class FkMethods : IFork
{
    private readonly IList<string> methods;
    private readonly IPoint point;
    
    public FkMethods(string method, IPoint point)
        : this(new ListOf<string>(method), point)
    {
        
    }

    public FkMethods(IList<string> methods, IPoint point)
    {
        this.methods = methods;
        this.point = point;
    }

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        var method = new RqMethod(req).Method();

        if (methods.Contains(method))
        {
            var res = await point.Act(req);
            return new Opt<IResponse>(res);
        }
        
        return new IOpt<IResponse>.Empty();
    }
}