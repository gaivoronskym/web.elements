using Point.Razor.Templates;
using RazorEngineCore;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Razor.Rs;

public class RsRazorView : RsRazor
{
    private readonly IRazorView _view;
    private readonly object _data;

    public RsRazorView(IRazorView view, object data)
    {
        _data = data;
        _view = view;
    }
    
    public override Stream Body()
    {
        IRazorEngine engine = new RazorEngine();

        var content = _view.Content();
        var template = engine.Compile(content, _view.Parts());
        var result = template.Run(_data);
        
        return new InputOf(new TextOf(result)).Stream();
    }
}