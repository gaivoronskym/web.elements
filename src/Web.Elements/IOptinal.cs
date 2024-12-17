namespace Web.Elements;

public interface IOptinal<T>
{
    bool Has();

    T Value();

    public sealed class Empty : IOptinal<T>
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