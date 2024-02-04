using Yaapii.Atoms.Text;

namespace Point.Razor;

public class RazorView : IRazorView
{
    private readonly string _path;

    public RazorView(string path)
    {
        _path = path;
    }

    public IDictionary<string, string> Parts()
    {
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", string.Concat(_path, ".cshtml"));
        var content = new TextOf(new Uri(fullPath)).AsString();
        IDictionary<string, string> parts = new Dictionary<string, string>()
        {
            {"outer", content},
        };

        return parts;
    }
    
    public string Content()
    {
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", string.Concat(_path, ".cshtml"));
        var content = new TextOf(new Uri(fullPath)).AsString();
        return content;
    }
}