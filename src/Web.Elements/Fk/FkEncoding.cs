using System.Text.RegularExpressions;
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
    
    private static readonly Regex encodingSplit = new Regex(@"(,)", RegexOptions.Compiled);
    private static readonly string header = "Accept-Encoding";
    
    public FkEncoding(string encoding, IResponse origin)
        : this(
            new Lower(
                new Trimmed(
                    new TextOf(
                        encoding
                    )
                )
            ),
            origin
        )
    {
    }
    
    public FkEncoding(IText encoding, IResponse origin)
    {
        this.encoding = encoding;
        this.origin = origin;
    }
    
    public Task<IOptional<IResponse>> Route(IRequest req)
    {
        var headers = new RqHeaders(req).Header(header);
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
                encodingSplit.Split(headers[0])
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

        return Task.FromResult(resp);
    }
}