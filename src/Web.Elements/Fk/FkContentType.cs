using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public sealed class FkContentType : IFork
{
    private readonly MediaTypes types;
    private readonly IPage page;
    private const string Header = "Content-Type";
    
    public FkContentType(string type, IResponse response)
        : this(type, new PageOf(response))
    {
    }
    
    public FkContentType(string type, IPage page)
    {
        this.types = new MediaTypes(type);
        this.page = page;
    }
    
    public async Task<IOptinal<IResponse>> Route(IRequest req)
    {
        IOptinal<IResponse> res;
        if (GetContentType(req).Contains(this.types))
        {
            res = new Optinal<IResponse>(await this.page.Act(req));
        }
        else
        {
            res = new IOptinal<IResponse>.Empty();
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