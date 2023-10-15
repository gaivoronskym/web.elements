using Point;
using Point.Authentication.Interfaces;
using Point.Authentication.Pt;
using Point.Fk;
using Point.Pt;
using Point.Rq.Interfaces;

namespace CustomServer;

public sealed class BookPoints : IFork
{
    private readonly IList<IFork> _forks;
    private readonly IPass _pass;

    public BookPoints(IPass pass)
    {
        _pass = pass;

        _forks = new List<IFork>
        {
            new FkRoute("/books",
                new PtMethod("GET",
                    new PtBooks()
                )
            ),
            
            new FkRoute("/lorem",
                new PtMethod("GET",
                    new PtLorem()
                )
            ),
            new FkRoute(@"/books/{bookId:\d+}/pages",
                new PtMethod("GET",
                    WithAuth(new PtBookPages())
                )
            ),
            new FkRoute(@"/books/{bookId:\d+}",
                new PtMethod("GET",
                    WithAuth(new PtBook())
                )
            ),
            new FkRoute(@"/books/{bookId:\d+}/html",
                new PtMethod("GET",
                    WithAuth(new PtBookHtml())
                )
            ),
            new FkRoute(@"/books/{bookId:\d+}/authors/{authorId:\d+}",
                new PtMethod("GET",
                    WithAuth(new PtBookAuthors())
                )
            )
        };
    }

    public async Task<IResponse?> Route(IRequest req)
    {
        foreach (var fork in _forks)
        {
            var response = await fork.Route(req);

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