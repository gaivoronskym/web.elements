using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace CustomServer;

public sealed class PtPostBook : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        var text = new TextOf(
            new InputOf(req.Body)
        ).AsString();

        return Task.FromResult<IResponse>(new RsJson(
                new JsonObject
                {
                    { "id", 1 }
                }
            )
        );
    }
}