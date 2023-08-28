using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rs;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace CustomServer;

public class PtPostBook : IPoint
{
    public IResponse Act(IRequest req)
    {
        var text = new TextOf(
            new InputOf(req.Body)
        ).AsString();
        
        return new RsJson(
            new JsonObject
            {
                { "id", 1 }
            }
        );
    }
}