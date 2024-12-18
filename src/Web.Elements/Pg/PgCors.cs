using System.Net;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Pg;

public sealed class PgCors : IPage
{
    private readonly IPage page;
    private readonly IEnumerable<string> allowed;

    public PgCors(IPage page, params string[] allowed)
    {
        this.page = page;
        this.allowed = allowed;
    }

    public async Task<IResponse> Act(IRequest req)
    {
        var origin = new IRqHeaders.Smart(req).Single("Origin", "");
        
        IResponse response;
        
        if (allowed.Contains(origin))
        {
            response = new RsWithHeaders(
                await this.page.Act(req),
                new TextOf("Access-Control-Allow-Credentials: true"),
                new TextOf("Access-Control-Allow-Methods: OPTIONS, GET, POST, PUT, DELETE, HEAD"),
                new Formatted("Access-Control-Allow-Origin: {0}", origin)
            );
        }
        else
        {
            response = new RsWithHeaders(
                new RsWithStatus(
                    HttpStatusCode.Forbidden
                ),
                "Access-Control-Allow-Credentials: false"
            );
        }

        return response;
    }
}