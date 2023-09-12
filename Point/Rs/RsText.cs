using Yaapii.Atoms.Text;

namespace Point.Rs;

public sealed class RsText : RsWrap
{
    public RsText(string text) :
        this(new RsWithBody(text))
    {
    }

    public RsText(IResponse origin) :
        base(
            new RsWithHeader(
                origin,
                new Formatted("{0}: {1}", "Content-Type", "text/plain").AsString()
            )
        )
    {

    }
}