using RazorEngineCore;

namespace Point.Razor.Templates;

public class Template : RazorEngineTemplateBase
{
    public Func<string, object, string> IncludeCallback { get; set; }
    public Func<string> RenderBodyCallback { get; set; }
    public string Layout { get; set; }

    public virtual string Include(string key, object model = null)
    {
        return this.IncludeCallback(key, model);
    }

    public virtual string RenderBody()
    {
        return this.RenderBodyCallback();
    }
}