using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point;

public sealed class BodyOf : IBody
{
    private readonly IScalar<Stream> stream;

    public BodyOf(Func<Stream> func)
        : this(new ScalarOf<Stream>(func))
    {
    }

    public BodyOf(Stream stream)
        : this(new ScalarOf<Stream>(() => stream))
    {
    }

    public BodyOf(IScalar<Stream> stream)
    {
        this.stream = stream;
    }

    public Stream Body()
    {
        return stream.Value();
    }
}