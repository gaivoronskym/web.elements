using System.Collections;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Fk;

public sealed class MediaTypes : IEnumerable<MediaType>
{
    private readonly IScalar<IList<MediaType>> src;

    public MediaTypes(string text)
        : this(new ScalarOf<IList<MediaType>>(() => new List<MediaType>(MediaTypes.Parse(text))))
    {
    }

    public MediaTypes(params MediaType[] mediaTypes)
        : this(new List<MediaType>(mediaTypes))
    {
    }
    
    public MediaTypes(IEnumerable<MediaType> mediaTypes)
        : this(new List<MediaType>(mediaTypes))
    {
    }

    public MediaTypes(IList<MediaType> mediaTypes)
        : this(new ScalarOf<IList<MediaType>>(() => mediaTypes))
    {
    }
    
    public MediaTypes(IScalar<IList<MediaType>> src)
    {
        this.src = src;
    }

    public bool Contains(MediaTypes types)
    {
        var contains = false;
        foreach (var mediaType in types)
        {
            if (this.Contains(mediaType))
            {
                contains = true;
                break;
            }
        }
        
        return contains;
    }
    
    public bool Contains(MediaType type)
    {
        var contains = false;
        foreach (var mediaType in this.src.Value())
        {
            if (mediaType.Matches(type))
            {
                contains = true;
                break;
            }
        }
        
        return contains;
    }

    public MediaTypes Merge(MediaTypes types)
    {
        return new MediaTypes(
            new Joined<MediaType>(
                this.src.Value(),
                types
            )
        );
    }
    
    public IEnumerator<MediaType> GetEnumerator()
    {
        return this.src.Value().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private static IEnumerable<MediaType> Parse(string text)
    {
        return new Mapped<string, MediaType>(
            i => new MediaType(i),
            new Filtered<string>(
                i => !string.IsNullOrEmpty(i),
                new Mapped<string, string>(
                    i => new Lower(
                        new TextOf(i)
                    ).AsString(),
                    new Split(
                        new TextOf(text),
                        ","
                    )
                )
            )
        );
    }
}