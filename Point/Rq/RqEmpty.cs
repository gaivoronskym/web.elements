using Yaapii.Atoms.IO;

namespace Point.Rq;

public sealed class RqEmpty : RqWrap
{
    public RqEmpty() : this("GET")
    {
        
    }

    public RqEmpty(string method) : this(method, "/ HTTP/1.1")
    {
        
    }

    public RqEmpty(string method, string query)
        : base(
            new RequestOf(
                new HeadOf(method),
                new BodyOf(
                    new InputStreamOf(string.Empty)
                )
            )
        )
    {
    }
}