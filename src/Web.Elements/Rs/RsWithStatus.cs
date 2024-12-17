using System.Net;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rs;

public sealed class RsWithStatus : RsWrap
{
    public RsWithStatus(HttpStatusCode code) 
        : this(new RsEmpty(), code)
    {
    }
    
    public RsWithStatus(IResponse origin, HttpStatusCode code) :
        base(
            new ResponseOf(
                Head(origin, code),
                origin.Body
            )
        )
    {
    }

    private static string Status(HttpStatusCode code)
    {
        return Statuses()[code];
    }
    
    private static IDictionary<HttpStatusCode, string> Statuses()
    {
        var dictionary = new Dictionary<HttpStatusCode, string>
        {
            { HttpStatusCode.OK, "OK" },
            { HttpStatusCode.Created, "Created" },
            { HttpStatusCode.Accepted, "Accepted" },
            { HttpStatusCode.NonAuthoritativeInformation, "Non-Authoritative Information" },
            { HttpStatusCode.NoContent, "No Content" },
            { HttpStatusCode.ResetContent, "Reset Content" },
            { HttpStatusCode.PartialContent, "Partial Content" },
            { HttpStatusCode.MultipleChoices, "Multiple Choices" },
            { HttpStatusCode.MovedPermanently, "Moved Permanently" },
            { HttpStatusCode.Found, "Found" },
            { HttpStatusCode.SeeOther, "See Other" },
            { HttpStatusCode.NotModified, "Not Modified" },
            { HttpStatusCode.UseProxy, "Use Proxy" },
            { HttpStatusCode.BadRequest, "Bad Request" },
            { HttpStatusCode.Unauthorized, "Unauthorized" },
            { HttpStatusCode.PaymentRequired, "Payment Required" },
            { HttpStatusCode.Forbidden, "Forbidden" },
            { HttpStatusCode.NotFound, "Not Found" },
            { HttpStatusCode.MethodNotAllowed, "Method Not Allowed" },
            { HttpStatusCode.NotAcceptable, "Not Acceptable" },
            { HttpStatusCode.ProxyAuthenticationRequired, "Proxy Authentication Required" },
            { HttpStatusCode.RequestTimeout, "Request Timeout" },
            { HttpStatusCode.Conflict, "Conflict" },
            { HttpStatusCode.Gone, "Gone" },
            { HttpStatusCode.LengthRequired, "Length Required" },
            { HttpStatusCode.PreconditionFailed, "Precondition Failed" },
            { HttpStatusCode.RequestEntityTooLarge, "Payload Too Large" },
            { HttpStatusCode.RequestUriTooLong, "URI Too Long" },
            { HttpStatusCode.UnsupportedMediaType, "Unsupported Media Type" },
            { HttpStatusCode.InternalServerError, "Internal Server Error" },
            { HttpStatusCode.NotImplemented, "Not Implemented" },
            { HttpStatusCode.BadGateway, "Bad Gateway" },
            { HttpStatusCode.ServiceUnavailable, "Service Unavailable" },
            { HttpStatusCode.GatewayTimeout, "Gateway Timeout" },
            { HttpStatusCode.HttpVersionNotSupported, "HTTP Version Not Supported" }
        };

        return dictionary;
    }

    private static IEnumerable<string> Head(IResponse origin, HttpStatusCode code)
    {
        return new Joined<string>(
            $"HTTP/1.1 {Convert.ToInt32(code)} {Status(code)}",
            origin.Head().Skip(1)
        );
    }
}