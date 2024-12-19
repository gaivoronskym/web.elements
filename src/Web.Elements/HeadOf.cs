using Yaapii.Atoms;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Scalar;

namespace Web.Elements;

public sealed class HeadOf : IHead
{
    private readonly IScalar<IEnumerable<string>> head;

    public HeadOf(string head)
        : this(new[] { head })
    {
    }

    public HeadOf(params string[] head)
        : this(new ListOf<string>(head))
    {
    }
    
    public HeadOf(IEnumerable<string> head)
        : this(new ScalarOf<IEnumerable<string>>(() => new ListOf<string>(head)))
    {
    }


    public HeadOf(IScalar<IEnumerable<string>> head)
    {
        this.head = head;
    }

    
    public IEnumerable<string> Head()
    {
        return this.head.Value();
    }
}