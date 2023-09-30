using Point;
using Point.Authentication.Interfaces;
using Point.Authentication.Pt;
using Point.Branch;
using Point.Pt;
using Point.Rq.Interfaces;

namespace CustomServer;

public sealed class BookPoints : IBranch
{
    private readonly IList<IBranch> _branches;
    private readonly IPass _pass;

    public BookPoints(IPass pass)
    {
        _pass = pass;

        _branches = new List<IBranch>
        {
            new BranchRoute("/books",
                new PtMethod("GET",
                    new PtBooks()
                )
            ),
            
            new BranchRoute("/lorem",
                new PtMethod("GET",
                    new PtLorem()
                )
            ),
            new BranchRoute(@"/books/{bookId:\d+}/pages",
                new PtMethod("GET",
                    WithAuth(new PtBookPages())
                )
            ),
            new BranchRoute(@"/books/{bookId:\d+}",
                new PtMethod("GET",
                    WithAuth(new PtBook())
                )
            ),
            new BranchRoute(@"/books/{bookId:\d+}/html",
                new PtMethod("GET",
                    WithAuth(new PtBookHtml())
                )
            ),
            new BranchRoute(@"/books/{bookId:\d+}/authors/{authorId:\d+}",
                new PtMethod("GET",
                    WithAuth(new PtBookAuthors())
                )
            )
        };
    }

    public async Task<IResponse?> Route(IRequest req)
    {
        foreach (var branch in _branches)
        {
            var response = await branch.Route(req);

            if (response is not null)
            {
                return response;
            }
        }

        return default;
    }

    private IPoint WithAuth(IPoint point)
    {
        return new PtAuth(point, _pass);
    }
}