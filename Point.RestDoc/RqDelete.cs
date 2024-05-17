namespace Point.RestDoc;

public sealed class RqDelete : Request
{
    public RqDelete(string summary, string operationId, IEnumerable<IDoc> docs)
        : base("delete", summary, operationId, docs)
    {
    }
}