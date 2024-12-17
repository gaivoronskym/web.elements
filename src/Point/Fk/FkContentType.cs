using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace Point.Fk;

public sealed class FkContentType : IFork
{
    private readonly MediaTypes types;
    private readonly IPoint point;
    private const string Header = "Content-Type";
    
    public FkContentType(string type, IResponse response)
        : this(type, new PointOf(response))
    {
    }
    
    public FkContentType(string type, IPoint point)
    {
        this.types = new MediaTypes(type);
        this.point = point;
    }
    
    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        IOpt<IResponse> res;
        if (GetContentType(req).Contains(this.types))
        {
            res = new Opt<IResponse>(await this.point.Act(req));
        }
        else
        {
            res = new IOpt<IResponse>.Empty();
        }

        return res;
    }
    
    private static MediaTypes GetContentType(IRequest req)
    {
        var list = new MediaTypes();
        var headers = new IRqHeaders.Base(req).Header(Header);
        
        foreach (var header in headers)
        {
            list = list.Merge(new MediaTypes(header));
        }

        if (!list.Any())
        {
            list = new MediaTypes("*/*");
        }
        
        return list;
    }
}