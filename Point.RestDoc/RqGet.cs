namespace Point.RestDoc;

public sealed class RqGet : Request
{
    public RqGet(string summary, string operationId, IEnumerable<IDoc> docs)
        : base("get", summary, operationId, docs)
    {
    }
}