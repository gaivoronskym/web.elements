using Point.Rq;
using Point.Rs;

namespace Point.Pg;

public sealed class PgEmpty : IPage
{
    public Task<IResponse> Act(IRequest req)
    {
        return Task.FromResult<IResponse>(new RsEmpty());
    }
}