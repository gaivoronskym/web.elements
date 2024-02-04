using CustomServer.Layouts;
using CustomServer.ViewModels;
using Point;
using Point.Pt;
using Point.Razor;
using Point.Razor.Rs;
using Point.Rq.Interfaces;
using Yaapii.Atoms.List;

namespace CustomServer;

public sealed class PtBooks : IPoint
{
    public Task<IResponse> Act(IRequest req)
    {
        BookListViewModel viewModel = new BookListViewModel(
            new ListOf<BookViewModel>(
                new BookViewModel("Object Thinking")
            )
        );

        var rs = new RsRazorView(
            new MainLayout(
                new RazorView("Books")
            ),
            viewModel
        );
        
        return Task.FromResult<IResponse>(rs);
    }
}