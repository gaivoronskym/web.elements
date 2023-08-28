namespace Point.Rs;

public class ResponseOf : IResponse
{
    private readonly IEnumerable<string> _head;
    private readonly Stream _body;

    public ResponseOf(IEnumerable<string> head, Stream body)
    {
        _head = head;
        _body = body;
    }

    public ResponseOf(Func<IEnumerable<string>> headFunc, Stream body)
    {
        _head = headFunc();
        _body = body;
    }

    public IEnumerable<string> Head()
    {
        return _head;
    }

    public Stream Body()
    {
        return _body;
    }
}