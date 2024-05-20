using System.Text.Json.Nodes;

namespace Point.RestDoc.Rq;

public sealed class RqResponses : DocWrap
{
    public RqResponses(params IDoc[] docs)
        : base(
            "responses",
            new JsonObject(
                new DocsMap(
                    docs
                )!
            )
        )
    {
    }
}