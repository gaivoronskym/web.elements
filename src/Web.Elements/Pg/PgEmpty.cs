using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Pg;

public sealed class PgEmpty : IPage
{
    public Task<IResponse> Act(IRequest req)
    {
        return Task.FromResult<IResponse>(new RsEmpty());
    }
}