namespace Point.Rs;

public sealed class ResponseOf : IResponse
{
    private readonly IEnumerable<string> _head;
    private readonly Func<Stream> _body;

    public ResponseOf(IEnumerable<string> head, Func<Stream> body)
    {
        _head = head;
        _body = body;
    }

    public ResponseOf(Func<IEnumerable<string>> headFunc, Func<Stream> body)
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
        return _body.Invoke();
    }
}