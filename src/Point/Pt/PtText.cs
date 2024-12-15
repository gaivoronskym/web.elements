using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Pt;

public sealed class PtText : IPoint
{
    private readonly string text;
    
    public PtText(string text)
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