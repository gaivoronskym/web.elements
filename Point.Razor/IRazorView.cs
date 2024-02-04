namespace Point.Razor;

public interface IRazorView
{
    IDictionary<string, string> Parts();

    string Content();
}