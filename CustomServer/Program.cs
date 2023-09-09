using Point.Authentication.Branch;
using Point.Authentication.Interfaces;
using Point.Authentication.Ps;
using Point.Authentication.Pt;
using Point.Backend;
using Point.Branch;
using Point.Pt;

namespace CustomServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IPass pass = new PsBearer("Server",
                "https://localhost",
                "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"
            );

            await new TcpBackend(
                    new PtBranch(
                        new BranchAuth(
                            pass,
                            new BranchPool(
                                new BranchRoute("/books", new PtMethod("GET", new PtBooks())),
                                new BranchRoute(@"/books/{bookId:\d+}/pages", new PtMethod("GET", new PtBookPages())),
                                new BranchRoute(@"/books/{bookId:\d+}", new PtMethod("GET", new PtBook())),
                                new BranchRoute(@"/books/{bookId:\d+}/html", new PtMethod("GET", new PtBookHtml())),
                                new BranchRoute(@"/books/{bookId:\d+}/authors/{authorId:\d+}", new PtMethod("GET", new PtBookAuthors()))
                            )
                        ),
                        new BranchRoute("/auth/login", new PtMethod("POST", new PtLogin()))
                    ),
                    5436)
                .StartAsync();
        }
    }
}