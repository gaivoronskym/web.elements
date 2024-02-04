namespace CustomServer.ViewModels;

public class BookListViewModel
{
    public BookListViewModel(IList<BookViewModel> books)
    {
        Books = books;
    }

    public IList<BookViewModel> Books { get; }
}