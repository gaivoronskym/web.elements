using System.Text;
using System.Text.RegularExpressions;
using Point.Backend;
using Point.Bind;

namespace CustomServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Backend(
                    new PtBranch(
                        // new BranchMethod("GET", "/books$", new PtBooks()),
                        // new BranchMethod("GET", "/books/[0-9]+$", new PtBook()),
                        new BranchMethod("GET", @"/books/{bookId:\d+}", new PtBook()),
                        new BranchMethod("GET", @"/books/{bookId:\d+}/authors/{authorId:\d+}", new PtBookAuthors())
                        // new BranchMethod("GET", "/books/[0-9]/authors/[0-9]+$", new PtBookAuthors()),
                        // new BranchMethod("POST", "/books", new PtPostBook())
                    ),
                    5436)
                .Start();
            
            //don't touch!!!
            // Regex regex = new Regex("/books[?&](([^&=]+)=([^&=#]*))");
            // var match = regex.IsMatch("/books");
            // Console.WriteLine(match);


            // Regex regex = new Regex(@":([^\\/]+)");
            //
            // var path = "/books/:bookId/authors/:authorId";
            //
            // var result = regex.Matches(path);

            // string pattern = @"(/(({(?<data>[^}/:]+)(:(?<type>[^}/]+))?}?)|(?<static>[^/]+))|\*)";
            // //  (/(({(?<data>[^}/:]+)(:(?<type>[^}/]+))?}?)|(?<static>[^/]+))|\*)
            //
            // Regex regex = new Regex(@"((?<static>[^/]+))(?<param>(((/({(?<data>[^}/:]+))?)(((:(?<type>[^}/]+))?)}))?))", RegexOptions.Compiled);
            //
            // var route = @"/books/{bookId:\d+}/authors/{authorId:\d+}";
            // //var match = regex.IsMatch(route);
            //
            // StringBuilder param = new StringBuilder();
            //
            // foreach (Match match in regex.Matches(route))
            // {
            //     param.Append($"{match.Groups["data"].Value},");
            //     route = route.Replace(match.Groups["param"].Value, $"/{match.Groups["type"]}");
            // }
            //
            // var t = $"path: {param}";
            //
            // regex.Replace(route, m =>
            // {
            //     if (string.IsNullOrEmpty(m.Groups["static"].Value) && !string.IsNullOrEmpty(m.Groups["data"].Value)
            //                                                        && !string.IsNullOrEmpty(m.Groups["type"].Value))
            //     {
            //         Regex.Match("", m.Groups["type"].Value);
            //     }
            //
            //     return null;
            // });
        }
    }
}