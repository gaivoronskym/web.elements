using System.Net;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public sealed class RqMethod : IRqMethod
{
    private readonly IRequest _origin;

    private readonly IList<string> _defaultMethods = new List<string>
    {
        "GET",
        "POST",
        "PUT",
        "PATCH"
    };

    public RqMethod(IRequest origin)
    {
        _origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return _origin.Head();
    }

    public Stream Body()
    {
        return _origin.Body();
    }

    public string Method()
    {
        var firstHeader = new ItemAt<string>(
            _origin.Head()
        ).Value();

        var method = new Split(firstHeader, " ").First();

        if (string.IsNullOrEmpty(method))
        {
            throw new HttpRequestException("Bad Request", null, HttpStatusCode.BadRequest);
        }

        return method;
    }
}