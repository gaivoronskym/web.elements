using System.Net;
using System.Text.RegularExpressions;
using Point.Exceptions;
using Yaapii.Atoms.Enumerable;

namespace Point.Rs;

public interface IRsStatus : IResponse
{
    int Status();
    
    public sealed class Base : IRsStatus
    {
        private static readonly Regex HeadPattern = new Regex("([!-~]+) ([^ ]+)( [^ ]+)?", RegexOptions.Compiled);
        
        private readonly IResponse origin;

        public Base(IResponse origin)
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
                    HeadPattern.Split(head)
                ).ToArray();
                return int.Parse(parts[1]);
            }
            catch (Exception e)
            {
                throw new HttpException(HttpStatusCode.InternalServerError, "Illegal response header");
            }
            
        }
    }
}