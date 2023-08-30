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
                        new BranchMethod("GET", @"/books", new PtBooks()),
                        new BranchMethod("GET", @"/books/{bookId:\d+}/pages", new PtBookPages()),
                        new BranchMethod("GET", @"/books/{bookId:\d+}", new PtBook()),
                        new BranchMethod("GET", @"/books/{bookId:\d+}/authors/{authorId:\d+}", new PtBookAuthors())
                    ),
                    5436)
                .Start();
        }
    }
}