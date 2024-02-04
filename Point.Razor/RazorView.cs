using Nito.AsyncEx;
using Point.Razor.Templates;
using RazorEngineCore;
using Yaapii.Atoms.Text;

namespace Point.Razor;

public class RazorView : IRazorView
{
    private readonly string _path;
    private readonly object _data;

    public RazorView(string path, object data)
    {
        _path = path;
        _data = data;
    }
    
    public string Content()
    {
        IRazorEngine engine = new RazorEngine();
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", string.Concat(_path, ".cshtml"));
        var content = new TextOf(new Uri(fullPath)).AsString();
        var template = AsyncContext.Run(() => engine.CompileAsync<Template>(content));
        var model = _data;
        var action = template.RunAsync(instance =>
        {
            if (!(model is AnonymousTypeWrapper))
            {
                model = new AnonymousTypeWrapper(model);
            }

            instance.Model = model;
        });
        return AsyncContext.Run(() => action);
    }
}