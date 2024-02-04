using RazorEngineCore;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Razor.Rs;

public class RsRazorView : RsRazor
{
    private readonly IRazorView _view;

    public RsRazorView(IRazorView view)
    {
        _view = view;
    }
    
    public override Stream Body()
    {
        var html = _view.Content();
        
        return new InputOf(
            new TextOf(
                html
            )
        ).Stream();
    }
}