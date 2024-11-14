using System.Text.Json.Nodes;
using Point.Fk;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Sample;

public sealed class PtBook : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        var form = new RqMultipart(req).Part("productName").First().Body();
        var temp = new TextOf(new InputOf(form)).AsString();
        
        var json = new JsonObject
        {
            { "Title", "Object Thinking" }
        };

        return new IResponse.Smart(
            new RsWithCookie(
                new RsJson(json),
                "Cookie",
                "Value"
            )
        ).AsTask();
    }
}