using System.Net;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public sealed class RqMethod : IRqMethod
{
    private readonly IRqRequestLine origin;

    public RqMethod(IRequest origin)
    {
        this.origin = new IRqRequestLine.Base(origin);
    }

    public IEnumerable<string> Head()
    {
        return origin.Head();
    }

    public Stream Body()
    {
        return origin.Body();
    }

    public string Method()
    {
        return this.origin.Method();
    }
}