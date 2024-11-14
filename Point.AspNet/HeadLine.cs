using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.AspNet;

public sealed class HeadLine : IKvp
{
    private readonly IScalar<string[]> head;

    public HeadLine(string head)
        : this(new ScalarOf<string>(() => head))
    {
    }

    public HeadLine(IScalar<string> head)
    {
        this.head = new ScalarOf<string[]>(
            () =>
            {
                var arr = head.Value().Split(":");

                if (arr.Length != 2)
                {
                    throw new ArgumentOutOfRangeException($"Invalid HTTP Header format. Parameter {head}");
                }

                return new[] { arr[0].Trim(), arr[1].Trim() };
            }
        );
    }

    public string Key()
    {
        return this.head.Value()[0];
    }

    public string Value()
    {
        return this.head.Value()[1];
    }

    public bool IsLazy()
    {
        return false;
    }
}