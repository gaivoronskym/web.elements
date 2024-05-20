namespace Point.RestDoc.Rq;

public sealed class RqPut : Request
{
    public RqPut(string summary, string operationId, IEnumerable<IDoc> docs)
        : base("put", summary, operationId, docs)
    {
    }
}