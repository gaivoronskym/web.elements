using System.Net;
using System.Text.RegularExpressions;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Point.Rq;

public class RqSinglePart : IRqSinglePart
{
    private readonly IRequest _origin;
    
    private readonly Regex _multipartHeaderRegex = new Regex(
        @"Content-Disposition: form-data; name=""((?<name>[^;]+)?)""((; filename=""(?<filename>[^w]+)"")?)(( Content-Type: (?<contentType>[^w]+))?)", RegexOptions.Compiled);

    public RqSinglePart(IRequest origin)
    {
        _origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return _origin.Head();
    }

    public Stream Body()
    {
        return _origin.Body();
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

        var matches = _multipartHeaderRegex.Matches(header);

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