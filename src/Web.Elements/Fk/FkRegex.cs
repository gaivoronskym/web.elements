using System.Text.RegularExpressions;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Func;

namespace Web.Elements.Fk;

public sealed class FkRegex : IFork
{
    private readonly Regex regex;
    private readonly IFunc<IRequest, Task<IResponse>> src;

    public FkRegex(string pattern, IPage page)
        : this(
            pattern,
            req => page.Act(req)
        )
    {
    }

    public FkRegex(string pattern, IPgRegex point)
        : this(
            new Regex(pattern, RegexOptions.Compiled),
            req => point.Act(new RqRegex(req, new Regex(pattern, RegexOptions.Compiled)))
        )
    {
    }
    
    public FkRegex(string pattern, Func<IRequest, Task<IResponse>> func)
        : this(
            new Regex(pattern, RegexOptions.Compiled),
            func
        )
    {
    }

    public FkRegex(Regex regex, Func<IRequest, Task<IResponse>> func)
        : this(
            regex,
            new FuncOf<IRequest, Task<IResponse>>(func)
        )
    {
    }

    public FkRegex(Regex regex, IFunc<IRequest, Task<IResponse>> src)
    {
        this.regex = regex;
        this.src = src;
    }

    public async Task<IOptional<IResponse>> Route(IRequest req)
    {
        var path = new RqHref(req).Href().LocalPath();
        if (this.regex.IsMatch(path))
        {
            var res = await src.Invoke(
                new RqRegex(
                    req,
                    this.regex.Match(path)
                )
            );

            return new Optional<IResponse>(res);
        }

        return new IOptional<IResponse>.Empty();
    }
}