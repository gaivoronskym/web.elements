using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public class PtBookAuthors : IPoint
{
    public IResponse Act(IRequest req)
    {
        var paramList = new RqUri(req).RouteParams();
        var query = new RqUri(req).Query();
        
        return new RsJson(
            new JsonObject
            {
                { "Author", "Author Name" }
            }
        );
    }
}