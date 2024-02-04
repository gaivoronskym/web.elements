using System.Net;
using Point.Rs;
using RazorEngineCore;

namespace Point.Razor.Rs;

public abstract class RsRazor : IResponse
{
    public IEnumerable<string> Head()
    {
        return new RsWithType(
            new RsWithStatus(HttpStatusCode.OK),
            "text/html"
        ).Head();
    }

    public abstract Stream Body();
}