namespace Point.RestDoc.Types;

public sealed class RqReference : RqTypeWrap
{
    public RqReference(string reference)
        : base("$ref", reference)
    {
        
    }
}