using System.Text.Json.Nodes;
using Point.Rs;

namespace Point.Pt;

public class PtBooks : IPoint
{
    public IResponse Act(IRequest req)
    {
        
        return new RsJson(
            new JsonObject
            {
                { "Title", "Object thinking" }
            }
        );
    }
}