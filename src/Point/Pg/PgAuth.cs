using Point.Rq;
using Point.Rs;

namespace Point.Pg;

public sealed class PgAuth : IPage
{
    private readonly IPage origin;
    private readonly IPass pass;
    private readonly string header;

    public PgAuth(IPage origin, IPass pass)
        : this(origin, pass, nameof(PgAuth))
    {
    }
    
    public PgAuth(IPage origin, IPass pass, string header)
    {
        this.origin = origin;
        this.pass = pass;
        this.header = header;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        var user = pass.Enter(req);

        if (!user.Has())
        {
            IRequest wrap = new RqWithoutHeader(req, header);
            return await origin.Act(wrap);
        }

        if (string.IsNullOrEmpty(user.Value().Identifier()))
        {
            IRequest wrap = new RqWithoutHeader(req, header);
            return await origin.Act(wrap);
        }
        
        return await Act(req, user.Value());
    }

    private async Task<IResponse> Act(IRequest req, IIdentity identity)
    {
        IRequest wrap = new RqWithoutHeader(req, header);
        if(!string.IsNullOrEmpty(identity.Identifier()))
        {
            wrap = new RqWithAuth(wrap, identity, header);
        }

        return pass.Exit(await origin.Act(wrap), identity);
    }
}