namespace Point.RestDoc.Rq;

public sealed class RqPatch : Request
{
    public RqPatch(string summary, string operationId, IEnumerable<IDoc> docs)
        : base("patch", summary, operationId, docs)
    {
    }
}