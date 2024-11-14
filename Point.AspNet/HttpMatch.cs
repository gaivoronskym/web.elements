using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Point.AspNet;

public sealed class HttpMatch : IScalar<bool>
{
    private const string Http = "HTTP";
    private readonly IScalar<bool> src;

    public HttpMatch(string head)
    {
        this.src = new StartsWith(new TextOf(head), Http);
    }

    public bool Value()
    {
        return this.src.Value();
    }
}