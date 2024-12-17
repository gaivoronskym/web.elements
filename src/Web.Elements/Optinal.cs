#nullable enable
namespace Web.Elements;

public sealed class Optinal<T> : IOptinal<T>
{
    private readonly T? value;

    public Optinal(T? value)
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