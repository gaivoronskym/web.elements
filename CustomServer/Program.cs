using Point.Backend;
using Point.Branch;
using Point.Pt;

namespace CustomServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // new Backend(
            //         new PtBranch(
            //             new BranchMethod("GET", @"/books", new PtBooks()),
            //             new BranchMethod("GET", @"/books/{bookId:\d+}/pages", new PtBookPages()),
            //             new BranchMethod("GET", @"/books/{bookId:\d+}", new PtBook()),
            //             new BranchMethod("GET", @"/books/{bookId:\d+}/authors/{authorId:\d+}", new PtBookAuthors())
            //         ),
            //         5436)
            //     .Start();

            new Backend(
                    new PtBranch(
                        new BranchRoute("/books", new PtMethod("GET", new PtBooks())),
                        new BranchRoute(@"/books/{bookId:\d+}/pages", new PtMethod("GET", new PtBookPages())),
                        new BranchRoute(@"/books/{bookId:\d+}", new PtMethod("GET", new PtBook())),
                        // new BranchRoute(@"/books/{bookId:\d+}/html", new PtMethod("GET", new PtBookHtml())),
                        new BranchRoute(@"/books/{bookId:\d+}/authors/{authorId:\d+}", new PtMethod("GET", new PtBookAuthors()))
                    ),
                    5436)
                .Start();
        }
    }
}