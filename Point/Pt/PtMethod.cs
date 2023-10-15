using Point.Fork;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;

namespace Point.Pt;

public sealed class PtMethod : PtWrap
{
    public PtMethod(string method, IPoint origin)
        : this(new ListOf<string>(method), origin)
    {
        
    }
    
    public PtMethod(IList<string> methods, IPoint origin)
        : base(
            new PtFork(
                new FkMethod(methods, origin)
            )
        )
    {
    }
}