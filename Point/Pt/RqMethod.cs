using System.Net;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;

namespace Point.Pt;

public class RqMethod : IRqMethod
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
        var method = new ItemAt<string>(
            new Filtered<string>(
                (item) => new Contains<string>(_defaultMethods, item).Value(),
                _origin.Head()
            )
        ).Value();

        if (string.IsNullOrEmpty(method))
        {
            throw new HttpRequestException("Bad Request", null, HttpStatusCode.BadRequest);
        }

        return method;
    }
}