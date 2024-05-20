using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;

namespace Point.RestDoc;

public sealed class Docs : ListEnvelope<IDoc>
{
    public Docs(params IDoc[] array) : this(new LiveMany<IDoc>(array))
    { }
    
    public Docs(IEnumerator<IDoc> src) : base(() => src, false)
    { }
    
    public Docs(IEnumerable<IDoc> src) : base(
        () => src.GetEnumerator(),
        false
    )
    { }
}