using Web.Elements.Codec;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

public class RqWithAuth : RqWrap
{
    public RqWithAuth(IRequest origin, IIdentity identity, string header)
        : base(
            new RqWithHeaders(
                origin,
                new Formatted(
                    "{0}: {1}",
                    header,
                    new TextOf(new CcPlain().Encode(identity)).AsString()
                ).AsString()
            )
        )
    {
    }
}