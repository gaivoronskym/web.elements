using Point;
using Point.Backend;
using Point.Bind;

namespace CustomServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IBackend backend = new Backend(
                new PtBind(
                    new BindRoute("/books", new PtBooks()),
                    new BindRoute("/books/authors", new PtBookAuthors()),
                    new BindRoute("/hello", "Hello, world")
                ),
                5436);

            backend.Start();

            // while (true)
            // {
            //     
            // }
        }
    }
}