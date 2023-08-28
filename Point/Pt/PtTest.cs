using System.Text.Json.Nodes;
using Point.Rs;

namespace Point.Pt;

public class PtTest : IPoint
{
    public IResponse Act(IRequest req)
    {
        
        return new RsJson(
            new JsonObject
            {
                { "Text", "Hello, world" }
            }
        );
    }
}