#nullable enable
namespace Web.Elements;

public sealed class Optional<T> : IOptional<T>
{
    private readonly T? value;

    public Optional(T? value)
    {
        this.value = value;
    }

    public bool Has()
    {
        return this.value is not null;
    }

    public T Value()
    {
        if (this.value is null)
        {
            throw new ArgumentNullException();
        }

        return this.value;
    }
}