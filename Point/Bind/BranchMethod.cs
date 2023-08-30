using System.Text;
using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using StringJoined = Yaapii.Atoms.Enumerable.Joined<string>;

namespace Point.Bind;

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