using Point.Pt;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms.Enumerable;

namespace Point.Fk;

public sealed class FkMethods : IFork
{
    private readonly IEnumerable<string> methods;
    private readonly IPoint point;
    
    public FkMethods(string method, IPoint point)
        : this(new ManyOf<string>(method.Split(',')), point)
    {
    }

    public FkMethods(IEnumerable<string> methods, IPoint point)
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