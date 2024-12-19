namespace Web.Elements;

public interface IOptional<T>
{
    bool Has();

    T Value();

    public sealed class Empty : IOptional<T>
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