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
            //         new PtBranch(
            //             new BranchMethod("GET", "/books$", new PtBooks()),
            //             new BranchMethod("GET", "/books/[0-9]+$", new PtBook()),
            //             new BranchMethod("GET", "/books/[0-9]/authors/[0-9]+$", new PtBookAuthors()),
            //             new BranchMethod("POST", "/books", new PtPostBook())
            //         ),
            //         5436)
            //     .Start();
            
            //don't touch!!!
            // Regex regex = new Regex("/books[?&](([^&=]+)=([^&=#]*))");
            // var match = regex.IsMatch("/books");
            // Console.WriteLine(match);


            Regex regex = new Regex(@":([^\\/]+)");

            var path = "/books/:bookId";

            var result = regex.Matches(path);

        }
    }
}