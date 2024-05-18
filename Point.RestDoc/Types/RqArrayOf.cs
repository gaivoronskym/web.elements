namespace Point.RestDoc.Types;

public sealed class RqArrayOf : RqTypeWrap
{
    public RqArrayOf(IRqType type)
        : base(
            new Docs(
                new DocOf("type", "array"),
                new DocOf("items", type.Docs())
            )
        )
    {

    }
}