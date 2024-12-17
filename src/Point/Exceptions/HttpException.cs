using System.Net;

namespace Point.Exceptions;

public class HttpException : Exception
{
    private readonly HttpStatusCode _statusCode;

    public HttpException(HttpStatusCode statusCode) : base(statusCode.ToString())
    {
        _statusCode = statusCode;
    }

    public HttpException(HttpStatusCode statusCode, string message) : base(message)
    {
        _statusCode = statusCode;
    }

    public HttpStatusCode Code()
    {
        return _statusCode;
    }
}