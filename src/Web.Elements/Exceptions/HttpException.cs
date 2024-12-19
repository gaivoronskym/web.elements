using System.Net;

namespace Web.Elements.Exceptions;

public class HttpException : Exception
{
    private readonly HttpStatusCode statusCode;

    public HttpException(HttpStatusCode statusCode) : base(statusCode.ToString())
    {
        this.statusCode = statusCode;
    }

    public HttpException(HttpStatusCode statusCode, string message) : base(message)
    {
        this.statusCode = statusCode;
    }

    public HttpStatusCode Code()
    {
        return statusCode;
    }
}