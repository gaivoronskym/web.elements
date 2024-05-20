using Yaapii.Atoms.List;

namespace Point.RestDoc.Types;

public abstract class RqTypeWrap : IRqType
{
    private readonly IEnumerable<IDoc> _docs;

    protected RqTypeWrap(IEnumerable<IDoc> docs)
    {
        _docs = docs;
    }

    protected RqTypeWrap(string key, string value)
        : this(
            new ListOf<IDoc>(
                new DocOf(key, value)
            )
        )
    {

    }

    protected RqTypeWrap(string value)
        : this(
            new ListOf<IDoc>(
                new DocOf("type", value)
            )
        )
    {

    }

    public ICollection<IDoc> Docs()
    {
        return _docs.ToList();
    }
}