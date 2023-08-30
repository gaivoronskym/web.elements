using Point.Pt;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;

namespace Point.Branch;

public class BranchMethod : IBranch
{
    private readonly IList<string> _methods;
    private readonly IPoint _point;
    
    public BranchMethod(string method, IPoint point)
        : this(new ListOf<string>(method), point)
    {
        
    }

    public BranchMethod(IList<string> methods, IPoint point)
    {
        _methods = methods;
        _point = point;
    }

    public IResponse? Route(IRequest req)
    {
        var method = new RqMethod(req).Method();

        if (_methods.Contains(method))
        {
            return _point.Act(req);
        }
        
        return default;
    }
}