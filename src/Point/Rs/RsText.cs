namespace Point.Rs;

public sealed class RsText : RsWrap
{
    public RsText(string text) :
        this(new RsWithBody(text))
    {
    }

    public RsText(IResponse origin) :
        base(
            new RsWithType(
                origin,
                "text/plain"
            )
        )
    {
    }
}