namespace Point.Razor.Templates;
using RazorEngineCore;

public class CompiledTemplate
{
    private readonly IRazorEngineCompiledTemplate<Template> compiledTemplate;
    private readonly Dictionary<string, IRazorEngineCompiledTemplate<Template>> compiledParts;

    public CompiledTemplate(IRazorEngineCompiledTemplate<Template> compiledTemplate,
        Dictionary<string, IRazorEngineCompiledTemplate<Template>> compiledParts)
    {
        this.compiledTemplate = compiledTemplate;
        this.compiledParts = compiledParts;
    }

    public string Run(object model)
    {
        return this.Run(this.compiledTemplate, model);
    }

    public string Run(IRazorEngineCompiledTemplate<Template> template, object model)
    {
        Template templateReference = null;

        string result = template.Run(instance =>
        {
            if (!(model is AnonymousTypeWrapper))
            {
                model = new AnonymousTypeWrapper(model);
            }

            instance.Model = model;
            instance.IncludeCallback = (key, includeModel) => this.Run(this.compiledParts[key], includeModel);

            templateReference = instance;
        });

        if (templateReference.Layout == null)
        {
            return result;
        }

        return this.compiledParts[templateReference.Layout].Run(instance =>
        {
            if (!(model is AnonymousTypeWrapper))
            {
                model = new AnonymousTypeWrapper(model);
            }

            instance.Model = model;
            instance.IncludeCallback = (key, includeModel) => this.Run(this.compiledParts[key], includeModel);
            instance.RenderBodyCallback = () => result;
        });
    }
}