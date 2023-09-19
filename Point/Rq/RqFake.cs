using Yaapii.Atoms.IO;
using Yaapii.Atoms.List;

namespace Point.Rq;

public sealed class RqFake : RqWrap
{

    public RqFake()
        : this("GET")
    {

    }

    public RqFake(string method)
        : this(method, "/ HTTP/1.1")
    {

    }

    public RqFake(string method, string query)
        : this(method, query, string.Empty)
    {

    }

    public RqFake(string method, string query, string body)
        : this(
            new ListOf<string>(
                $"{method} {query}",
                "Host: www.example.com"
            ),
            body
        )
    {

    }

    public RqFake(IEnumerable<string> head, string body)
        : this(head, new InputStreamOf(body))
    {

    }

    public RqFake(IEnumerable<string> head, byte[] bytes)
        : this(head, new InputStreamOf(bytes))
    {

    }

    public RqFake(IEnumerable<string> head, Stream stream)
        : base(new RequestOf(head, stream))
    {
    }
}