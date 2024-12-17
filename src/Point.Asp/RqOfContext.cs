using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Point.Rq;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Point.Asp;

public sealed class RqOfContext : IRequest
{
    private readonly IScalar<IEnumerable<string>> head;
    private readonly IScalar<Stream> body;

    public RqOfContext(HttpRequest req)
    {
        this.head = new ScalarOf<IEnumerable<string>>(
            () =>
            {
                return new Joined<string>(
                    $"{req.Method} {req.Path} {req.Protocol}",
                    new Mapped<KeyValuePair<string, StringValues>, string>(
                        i => $"{i.Key}: {i.Value}",
                        req.Headers
                    )
                );
            }
        );

        this.body = new ScalarOf<Stream>(() =>
        {
            var stream = new MemoryStream();
            req.BodyReader.AsStream().CopyTo(stream);

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        });
    }

    public IEnumerable<string> Head()
    {
        return this.head.Value();
    }

    public Stream Body()
    {
        return this.body.Value();
    }
}