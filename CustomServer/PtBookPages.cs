using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public sealed class PtBookPages : IPoint
{
    public IResponse Act(IRequest req)
    {
        var query = new RqUri(req).Query();
        
        return new RsJson(
            new JsonArray(
                    new JsonObject
                    {
                        { "PageNumber", "1" }
                    },
                    new JsonObject
                    {
                        { "PageNumber", "2" }
                    },
                    new JsonObject
                    {
                        { "PageNumber", "3" }
                    }
                )
        );
    }
}