using Point.Rq;
using Point.Rq.Interfaces;

namespace Point.Fk;

public sealed class FkTypes : IFork
{
    private readonly string type;
    private readonly IResponse response;
    private const string AcceptAll = "*/*";
    
    public FkTypes(string type, IResponse response)
    {
        this.type = type;
        this.response = response;
    }

    public Task<IOpt<IResponse>> Route(IRequest req)
    {
        var acceptHeader = new IRqHeaders.Base(req).Header("Accept");

        if (acceptHeader.Contains(AcceptAll))
        {
            return Task.FromResult<IOpt<IResponse>>(
                new Opt<IResponse>(
                    response
                )
            );
        }
        
        if (acceptHeader.Contains(type))
        {
            return Task.FromResult<IOpt<IResponse>>(
                new Opt<IResponse>(
                    response
                )
            );
        }

        return Task.FromResult<IOpt<IResponse>>(
            new IOpt<IResponse>.Empty()
        );
    }
}