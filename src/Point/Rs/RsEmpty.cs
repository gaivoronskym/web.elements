using Yaapii.Atoms.IO;
using Yaapii.Atoms.List;

namespace Point.Rs;

public sealed class RsEmpty : IResponse
{
    public IEnumerable<string> Head()
    {
        return new ListOf<string>("HTTP/1.1 204 No Content");
    }

    public Stream Body()
    {
        return new InputStreamOf(string.Empty);
    }
}