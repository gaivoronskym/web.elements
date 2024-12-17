using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public sealed class FkTypes : IFork
{
    private readonly MediaTypes types;
    private readonly IPage page;
    private const string Header = "Accept";
    
    public FkTypes(string type, IResponse response)
        : this(type, new PageOf(response))
    {
    }
    
    public FkTypes(string type, IPage page)
    {
        this.types = new MediaTypes(type);
        this.page = page;
    }

    public async Task<IOptional<IResponse>> Route(IRequest req)
    {
        IOptional<IResponse> res;
        if (Accepted(req).Contains(this.types))
        {
            res = new Optional<IResponse>(await this.page.Act(req));
        }
        else
        {
            res = new IOptional<IResponse>.Empty();
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