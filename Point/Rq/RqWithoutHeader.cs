using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Rq
{
    public class RqWithoutHeader : RqWrap
    {
        public RqWithoutHeader(IRequest origin, string header)
             : base(
                    new RequestOf(
                            () => new Filtered<string>
                                (
                                    (input) => new Not(
                                                new StartsWith(
                                                new TextOf(input),
                                                header
                                            )
                                        ).Value(),
                                    origin.Head()
                                )
                            ,
                            () => origin.Body()
                        )
                   )
        {

        }
    }
}
