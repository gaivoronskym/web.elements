using Nito.AsyncEx;
using Point.Razor.Templates;
using RazorEngineCore;
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

    public string Content()
    {
        IRazorEngine engine = new RazorEngine();
        
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", string.Concat(_path, ".cshtml"));
        var content = new TextOf(new Uri(fullPath)).AsString();

        var template = AsyncContext.Run(() => engine.CompileAsync<Template>(content));

        var result = template.Run(instance =>
        {
            instance.RenderBodyCallback = () => _origin.Content();
        });

        return result;
    }
}