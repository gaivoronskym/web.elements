using System.Net;
using System.Text.RegularExpressions;
using Web.Elements.Exceptions;
using Yaapii.Atoms.Enumerable;

namespace Web.Elements.Rs;

public sealed class RsStatus : IRsStatus
{
    private static readonly Regex headPattern = new Regex("([!-~]+) ([^ ]+)( [^ ]+)?", RegexOptions.Compiled);
        
    private readonly IResponse origin;

    public RsStatus(IResponse origin)
    {
        this.origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return this.origin.Head();
    }

    public Stream Body()
    {
        return this.origin.Body();
    }

    public int Status()
    {
        try
        {
            var head = this.Head().First();
            var parts = new Filtered<string>(
                i => !string.IsNullOrEmpty(i),
                headPattern.Split(head)
            ).ToArray();
            return int.Parse(parts[1]);
        }
        catch (Exception )
        {
            throw new HttpException(HttpStatusCode.InternalServerError, "Illegal response header");
        }
            
    }
}