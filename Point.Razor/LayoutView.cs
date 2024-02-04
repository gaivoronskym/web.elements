using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;

namespace Point.Razor;

public abstract class LayoutView : IRazorView
{
    private readonly IRazorView _origin;
    private readonly string _path;

    protected LayoutView(IRazorView origin, string path)
    {
        _origin = origin;
        _path = path;
    }

    public IDictionary<string, string> Parts()
    {
        var layout = new KvpOf<string>("Layout", "@RenderBody()");

        var body = _origin.Parts();
        IDictionary<string, string> parts = new Dictionary<string, string>();
        
        parts.Add(layout.Key(), layout.Value());
        foreach (var bodyItem in body)
        {
            parts.Add(bodyItem);
        }

        return parts;
    }

    public string Content()
    {
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", string.Concat(_path, ".cshtml"));
        var content = new TextOf(new Uri(fullPath)).AsString();
        return content;
    }
}