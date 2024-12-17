﻿using System.Text.RegularExpressions;
using Web.Elements.Rq;
using Web.Elements.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Web.Elements.Fk;

public sealed class FkEncoding : IFork
{
    private readonly IText encoding;
    private readonly IResponse origin;
    
    private static Regex EncodingSplit = new Regex(@"(,)", RegexOptions.Compiled);

    public FkEncoding(string encoding, IResponse origin)
    {
        this.encoding = new Lower(
            new Trimmed(
                new TextOf(
                    encoding
                )
            )
        );
        this.origin = origin;
    }

    public Task<IOptional<IResponse>> Route(IRequest req)
    {
        var headers = new IRqHeaders.Base(req).Header("Accept-Encoding");
        IOptional<IResponse> resp;
        if (string.IsNullOrEmpty(this.encoding.AsString()))
        {
            resp = new Optional<IResponse>(this.origin);
        }
        else if (headers.Any())
        {
            var requested = new Mapped<string, string>(
                i => new Lower(
                    new Trimmed(
                        new TextOf(
                            i
                        )
                    )
                ).AsString(),
                EncodingSplit.Split(headers[0])
            );
            
            if (requested.Contains(this.encoding.AsString()))
            {
                resp = new Optional<IResponse>(this.origin);
            }
            else
            {
                resp = new IOptional<IResponse>.Empty();
            }
        }
        else
        {
            resp = new IOptional<IResponse>.Empty();
        }

        return Task.FromResult<IOptional<IResponse>>(resp);
    }
}