using System.Text.Json.Nodes;
using Point.Fk;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace Point.Sample;

public sealed class PtBook : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        
        var json = new JsonObject
        {
            { "Title", "Object Thinking" }
        };

        return Task.FromResult<IResponse>(
            new RsFork(
                req,
                new FkTypes(
                    "application/json",
                    new RsJson(json)
                ),
                new FkTypes("text/html",
                    new RsHtml(
                        """<html><head><meta name="color-scheme" content="light dark"></head><body><pre style="word-wrap: break-word; white-space: pre-wrap;">Title: Object thinking</pre></body></html>"""
                    )
                )
            )
        );
    }
}