using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.AspNet;

public sealed class StatusCodeOf : IScalar<int>
{
    private readonly Ternary<IResponse, int> src;

    public StatusCodeOf(IResponse response)
    {
        this.src = new Ternary<IResponse, int>(
            new HttpMatch(response.Head().First()),
            new ScalarOf<int>(() =>
            {
                var head = response.Head().First();
                var arr = head.Split(" ");
                return Convert.ToInt32(arr[1].Trim());
            }),
            new ScalarOf<int>(() => 200)
        );
    }

    public int Value()
    {
        return this.src.Value();
    }
}