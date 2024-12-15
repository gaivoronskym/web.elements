namespace Point.Rs;

public sealed class ResponseOf : IResponse
{
    private readonly Func<IEnumerable<string>> head;
    private readonly Func<Stream> body;
    
    public ResponseOf(IEnumerable<string> head, Func<Stream> body)
        : this(() => head, body)
    {
    }

    public ResponseOf(Func<IEnumerable<string>> head, Func<Stream> body)
    {
        this.head = head;
        this.body = body;
    }

    public IEnumerable<string> Head()
    {
        return head();
    }

    public Stream Body()
    {
        return body();
    }
}