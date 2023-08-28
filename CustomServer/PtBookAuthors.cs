using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rs;

namespace CustomServer;

public class PtBookAuthors : IPoint
{
    public IResponse Act(IRequest req)
    {
        return new RsJson(
            new JsonObject
            {
                { "Author", "Author Name" }
            }
        );
    }
}