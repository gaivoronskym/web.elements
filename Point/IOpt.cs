namespace Point;

public interface IOpt<T>
{
    bool Has();

    T Value();

    public sealed class Empty : IOpt<T>
    {
        public Empty() { }

        public bool Has()
        {
            return false;
        }

        public T Value()
        {
            throw new NotImplementedException();
        }
    }
}