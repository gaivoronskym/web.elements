using System.Net;
using Yaapii.Atoms.List;

namespace Point.Exceptions;

public class HttpCorsException : Exception
{
    private readonly HttpStatusCode _statusCode;
    private readonly IEnumerable<string> _head;

    public HttpCorsException(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        _head = new ListOf<string>();
    }
    
    public HttpCorsException(HttpStatusCode statusCode, IEnumerable<string> head)
    {
        _statusCode = statusCode;
        _head = head;
    }

    public HttpStatusCode Status()
    {
        return _statusCode;
    }

    public IHead Head()
    {
        return new HeadOf(_head);
    }
}