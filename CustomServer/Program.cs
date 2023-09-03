using Point.Backend;
using Point.Branch;
using Point.Pt;

namespace CustomServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new TcpBackend(
                    new PtBranch(
                        new BranchRoute("/books", new PtMethod("GET", new PtBooks())),
                        new BranchRoute("/auth/login", new PtMethod("POST", new PtLogin())),
                        new BranchRoute(@"/books/{bookId:\d+}/pages", new PtMethod("GET", new PtBookPages())),
                        new BranchRoute(@"/books/{bookId:\d+}", new PtMethod("GET", new PtBook())),
                        new BranchRoute(@"/books/{bookId:\d+}/html", new PtMethod("GET", new PtBookHtml())),
                        new BranchRoute(@"/books/{bookId:\d+}/authors/{authorId:\d+}", new PtMethod("GET", new PtBookAuthors()))
                    ),
                    5436)
                .Start();
        }
    }
}