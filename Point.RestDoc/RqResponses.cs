using System.Text.Json.Nodes;

namespace Point.RestDoc;

public sealed class RqResponses : DocWrap
{
    public RqResponses(params IDoc[] docs)
        : base(
            "responses",
            () =>
            {
                var json = new JsonObject();
                foreach (var doc in docs)
                {
                    json.Add(doc.Key(), doc.Content());
                }

                return json;
            }
        )
    {
    }
}