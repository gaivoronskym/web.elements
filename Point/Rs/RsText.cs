namespace Point.Rs;

public class RsText : RsWrap
{
    public RsText(string text) :
        base(new RsWithBody(text))
    {
    }
}