namespace Point.RestDoc.Rq;

public sealed class RqPost : Request
{
    public RqPost(string summary, string operationId, IEnumerable<IDoc> docs)
        : base("post", summary, operationId, docs)
    {
    }
}