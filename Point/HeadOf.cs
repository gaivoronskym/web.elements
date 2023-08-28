using Yaapii.Atoms.List;

namespace Point;

public class HeadOf : IHead
{
    private readonly IEnumerable<string> _head;

    public HeadOf(string head)
    {
        _head = new ListOf<string>(head);
    }

    public HeadOf(params string[] head)
    {
        _head = new ListOf<string>(head);
    }
    
    public HeadOf(IEnumerable<string> head)
    {
        _head = new ListOf<string>(head);
    }
    
    public IEnumerable<string> Head()
    {
        return _head;
    }
}