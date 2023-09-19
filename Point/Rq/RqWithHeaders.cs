using Point.Rq.Interfaces;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public class RqWithHeaders : RqWrap
{
    public RqWithHeaders(IRequest origin, params string[] headers)
        : base(
            new RequestOf(
                () =>
                {
                    IList<string> head = new List<string>();
                    foreach (var h in origin.Head())
                    {
                        head.Add(h);
                    }

                    foreach (var header in headers)
                    {
                        head.Add(
                            new Trimmed(
                                header
                            ).AsString()
                        );
                    }

                    return head;
                },
                origin.Body
            )
        )
    {
    }
}