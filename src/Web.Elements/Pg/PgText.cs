using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Pg;

public sealed class PgText : IPage
{
    private readonly string text;
    
    public PgText(string text)
    {
        this.text = text;
    }

    public Task<IResponse> Act(IRequest req)
    {
        return Task.FromResult<IResponse>(
            new RsText(text)
        );
    }
}