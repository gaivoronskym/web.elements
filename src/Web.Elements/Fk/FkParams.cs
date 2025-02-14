﻿using System.Text.RegularExpressions;
using Web.Elements.Pg;
using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Fk;

public sealed class FkParams : IFork
{
    private readonly string name;
    private readonly Regex regex;
    private readonly IPage page;

    public FkParams(string name, string pattern, IPage page)
        : this(name, new Regex(pattern, RegexOptions.Compiled), page)
    {
    }

    public FkParams(string name, Regex regex, IPage page)
    {
        this.name = name;
        this.regex = regex;
        this.page = page;
    }

    public async Task<IOptional<IResponse>> Route(IRequest req)
    {
        IOptional<IResponse> res;
        var queryParams = new RqHref(req).Href().Param(this.name);

        if (queryParams.Count > 0 && regex.IsMatch(queryParams[0]))
        {
            res = new Optional<IResponse>(await this.page.Act(req));
        }
        else
        {
            res = new IOptional<IResponse>.Empty();
        }

        return res;
    }
}