using System.Text.Json.Nodes;
using Point;
using Point.Branch;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public sealed class PtBook : IPoint
{
    public IResponse Act(IRequest req)
    {
        var paramList = new RqUri(req).RouteParams();
        
        var json = new JsonObject
        {
            { "Title", "Object Thinking" }
        };

        return new RsBranch(
            req,
            new BranchTypes("application/json", new RsJson(json)),
            new BranchTypes("text/html", new RsHtml("""<html><head><meta name="color-scheme" content="light dark"></head><body><pre style="word-wrap: break-word; white-space: pre-wrap;">Title: Object thinking</pre></body></html>"""))
        );
    }
}