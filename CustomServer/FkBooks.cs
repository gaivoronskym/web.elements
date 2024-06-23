using Point;
using Point.Authentication.Pt;
using Point.Fk;
using Point.Pt;
using Point.Rq.Interfaces;

namespace CustomServer;

public sealed class FkBooks : IFork
{
    private readonly IList<IFork> _forks;

    public FkBooks()
    {
        _forks = new List<IFork>
        {
            new FkRoute("/books",
                new PtMethod("GET",
                    WithAuth(new PtBooks())
                )
            ),
            
            new FkRoute("/lorem",
                new PtMethod("GET",
                    WithAuth(new PtLorem())
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

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        foreach (var fork in _forks)
        {
            var response = await fork.Route(req);

            return response;
        }

        return new IOpt<IResponse>.Empty();
    }

    private IPoint WithAuth(IPoint point)
    {
        return new PtAuthenticated(point, "Authorization");
    }
}