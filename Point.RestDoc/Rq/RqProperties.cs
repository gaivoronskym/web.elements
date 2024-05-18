using System.Text.Json.Nodes;
using Yaapii.Atoms.Map;

namespace Point.RestDoc.Rq;

public sealed class RqProperties : DocWrap
{
    public RqProperties(params IDoc[] docs)
        : base(
            "properties",
            new JsonObject(
                new DocsMap(
                    docs
                )!
            )
        )
    {

    }
}