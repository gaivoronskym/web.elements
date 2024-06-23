namespace Point;

public sealed class Opt<T> : IOpt<T>
{
    private readonly T? value;

    public Opt(T? value)
    {
        this.value = value;
    }

    public bool IsEmpty()
    {
        return this.value is null;
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