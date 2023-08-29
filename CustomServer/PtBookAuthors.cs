using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace CustomServer;

public class PtBookAuthors : IPoint
{
    public IResponse Act(IRequest req)
    {
        var paramList = new RqUri(req).RouteParams();
        
        return new RsJson(
            new JsonObject
            {
                { "Author", "Author Name" }
            }
        );
    }
}