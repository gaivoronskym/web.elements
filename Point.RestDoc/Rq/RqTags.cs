using System.Text.Json.Nodes;

namespace Point.RestDoc.Rq;

public sealed class RqTags : IDoc
{
    private readonly IEnumerable<string> _tags;

    public RqTags(params string[] tags)
    {
        _tags = tags;
    }

    public string Key()
    {
        return "tags";
    }

    public JsonNode Content()
    {
        var array = new JsonArray();

        foreach (var tag in _tags)
        {
            array.Add(tag);
        }

        return array;
    }
}