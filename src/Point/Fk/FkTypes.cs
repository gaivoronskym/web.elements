using Point.Pt;
using Point.Rq.Interfaces;

namespace Point.Fk;

public sealed class FkTypes : IFork
{
    private readonly MediaTypes types;
    private readonly IPoint point;
    private const string Header = "Accept";
    
    public FkTypes(string type, IResponse response)
        : this(type, new PointOf(response))
    {
    }
    
    public FkTypes(string type, IPoint point)
    {
        this.types = new MediaTypes(type);
        this.point = point;
    }

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        IOpt<IResponse> res;
        if (Accepted(req).Contains(this.types))
        {
            res = new Opt<IResponse>(await this.point.Act(req));
        }
        else
        {
            res = new IOpt<IResponse>.Empty();
        }

        return res;
    }

    private static MediaTypes Accepted(IRequest req)
    {
        var list = new MediaTypes();
        var headers = new IRqHeaders.Base(req).Header(Header);
        
        foreach (var header in headers)
        {
            list = list.Merge(new MediaTypes(header));
        }

        if (!list.Any())
        {
            list = new MediaTypes("text/html");
        }
        
        return list;
    }
}