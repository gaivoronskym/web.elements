using System.Text.RegularExpressions;
using Point.Backend;
using Point.Bind;

namespace CustomServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // new Backend(
            //         new PtBind(
            //             new BindMethod("GET", "/books$", new PtBooks()),
            //             new BindMethod("GET", "/books/[0-9]+$", new PtBook()),
            //             new BindMethod("GET", "/books/[0-9]/authors/[0-9]+$", new PtBookAuthors()),
            //             new BindMethod("POST", "/books", new PtPostBook())
            //         ),
            //         5436)
            //     .Start();
            
            Regex regex = new Regex("/books[?&](([^&=]+)=([^&=#]*))");
            var match = regex.IsMatch("/books");
            
            
            Console.WriteLine(match);
        }
    }
}