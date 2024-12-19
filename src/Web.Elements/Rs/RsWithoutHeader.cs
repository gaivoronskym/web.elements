using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rs;

public class RsWithoutHeader : RsWrap
{
    public RsWithoutHeader(IResponse origin, string name)
        : base(
            new ResponseOf(
                () => new Filtered<string>(
                    head => new Not(
                        new StartsWith(
                            new Lower(new TextOf(head)),
                            new Lower(new Formatted("{0}:", name))
                        )
                    ).Value(),
                    origin.Head()
                ),
                origin.Body
            )
        )
    {
    }
}