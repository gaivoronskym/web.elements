using Yaapii.Atoms.List;

namespace Point;

public sealed class HeadOf : IHead
{
    private readonly IEnumerable<string> head;

    public HeadOf(string head)
    {
        this.head = new ListOf<string>(head);
    }

    public HeadOf(params string[] head)
    {
        this.head = new ListOf<string>(head);
    }
    
    public HeadOf(IEnumerable<string> head)
    {
        this.head = new ListOf<string>(head);
    }
    
    public IEnumerable<string> Head()
    {
        return this.head;
    }
}