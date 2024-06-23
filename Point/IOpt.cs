namespace Point;

public interface IOpt<T>
{
    bool IsEmpty();

    T Value();

    public sealed class Empty : IOpt<T>
    {
        public Empty() { }

        public bool IsEmpty()
        {
            return true;
        }

        public T Value()
        {
            throw new NotImplementedException();
        }
    }
}