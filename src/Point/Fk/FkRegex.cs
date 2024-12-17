using System.Text.RegularExpressions;
using Point.Pt;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Func;

namespace Point.Fk;

public sealed class FkRegex : IFork
{
    private readonly Regex regex;
    private readonly IFunc<IRequest, Task<IResponse>> point;

    public FkRegex(string pattern, IPoint point)
        : this(
            pattern,
            req => point.Act(req)
        )
    {
    }

    public FkRegex(string pattern, IPtRegex point)
        : this(
            new Regex(pattern, RegexOptions.Compiled),
            req => point.Act(new IRqRegex.Fake(req, new Regex(pattern, RegexOptions.Compiled)))
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

    public FkRegex(Regex regex, IFunc<IRequest, Task<IResponse>> point)
    {
        this.regex = regex;
        this.point = point;
    }

    public async Task<IOpt<IResponse>> Route(IRequest req)
    {
        var path = new IRqHref.Base(req).Href().LocalPath();
        if (this.regex.IsMatch(path))
        {
            var res = await point.Invoke(
                new IRqRegex.Fake(
                    req,
                    this.regex.Match(path)
                )
            );

            return new Opt<IResponse>(res);
        }

        return new IOpt<IResponse>.Empty();
    }
}