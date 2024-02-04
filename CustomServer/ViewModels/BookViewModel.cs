namespace CustomServer.ViewModels;

public class BookViewModel
{
    public BookViewModel(string title)
    {
        Title = title;
    }
    
    public string Title { get; }
}