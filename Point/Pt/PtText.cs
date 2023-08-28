using Point.Rs;

namespace Point.Pt;

public class PtText : IPoint
{
    private readonly string _text;
    
    public PtText(string text)
    {
        _text = text;
    }

    public IResponse Act(IRequest req)
    {
        return new RsText(_text);
    }
}