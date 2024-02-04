using Point.Razor.Templates;
using RazorEngineCore;

namespace Point.Razor;

public static class RazorEngineExtensions
{
    public static CompiledTemplate Compile(this IRazorEngine razorEngine, string template, IDictionary<string, string> parts)
    {
        return new CompiledTemplate(
            razorEngine.Compile<Template>(template),
            parts.ToDictionary(
                k => k.Key,
                v => razorEngine.Compile<Template>(v.Value)));
    }

    private static void ConfigureOpts(IRazorEngineCompilationOptionsBuilder opts)
    {
        opts.AddAssemblyReference(typeof(Math));
        opts.AddAssemblyReferenceByName("System.Collections");
        opts.AddUsing("System");
        opts.AddUsing("System.Collections");
    }
}