using System.Net;
using System.Text.RegularExpressions;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

public sealed class RqSinglePart : IRqSinglePart
{
    private readonly IRequest origin;
    
    private readonly Regex multipartHeaderRegex = new Regex(
        @"Content-Disposition: form-data; name=""((?<name>[^;]+)?)""((; filename=""(?<filename>[^w]+)"")?)(( Content-Type: (?<contentType>[^w]+))?)", RegexOptions.Compiled);

    public RqSinglePart(IRequest origin)
    {
        this.origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return origin.Head();
    }

    public Stream Body()
    {
        return origin.Body();
    }

    public string PartName()
    {
        var header = new ItemAt<string>(
            new Filtered<string>(
                (item) => new StartsWith(
                    new TextOf(item),
                    "Content-Disposition"
                ).Value(),
                Head()
            )
        ).Value();

        var matches = multipartHeaderRegex.Matches(header);

        if (matches.Count == 0)
        {
            throw new HttpRequestException(
                "Bad Request",
                null,
                HttpStatusCode.BadRequest
            );
        }
        
        var name = matches.First().Groups["name"].Value;

        return name;
    }
}