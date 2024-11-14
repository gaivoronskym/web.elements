using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

namespace Point.AspNet;

public sealed class RqAspNet : IRequest
{
    private readonly Live<IEnumerable<string>> head;
    private readonly Live<Stream> body;

    public RqAspNet(HttpRequest req)
    {
        this.head = new Live<IEnumerable<string>>(
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

        this.body = new Live<Stream>(() =>
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