using System.Net;
using System.Text.RegularExpressions;
using Web.Elements.Exceptions;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

public interface IRqRequestLine : IRequest
{
    string Header();

    string Method();

    string Uri();

    string Version();

    public sealed class Base : RqWrap, IRqRequestLine
    {
        private static readonly Regex pattern = new Regex("([!-~]+) ([^ ]+)( [^ ]+)?", RegexOptions.Compiled);

        private static string badRequest = "Invalid HTTP Request-Line header: {0}";

        private enum HttpToken
        {
            method = 1,
            
            uri = 2,
            
            version = 3
        }
        
        public Base(IRequest origin) : base(origin)
        {
        }

        public string Header()
        {
            return IRqRequestLine.Base.Validated(this.Line());
        }

        public string Method()
        {
            return this.Token(HttpToken.method);
        }

        public string Uri()
        {
            return this.Token(HttpToken.uri);
        }

        public string Version()
        {
            return this.Token(HttpToken.version);
        }

        private string Line()
        {
            var head = this.Head().ToList();
            if (!head.Any())
            {
                throw new HttpException(
                    HttpStatusCode.BadRequest,
                    "HTTP Request should have Request-Line"
                );
            }

            return new ItemAt<string>(head).Value();
        }

        private string Token(HttpToken token)
        {
            return Trimmed(
                IRqRequestLine.Base.Match(this.Line()).Groups[TokenValue(token)].Value,
                token
            );
        }

        private static int TokenValue(HttpToken token)
        {
            return Convert.ToInt32(token);
        }
        
        private static Match Match(string line)
        {
            if (!pattern.IsMatch(line))
            {
                throw new HttpException(
                    HttpStatusCode.BadRequest,
                    string.Format(badRequest, line)
                );
            }
            
            return pattern.Match(line);
        }

        private static string Validated(string line)
        {
            if (!pattern.IsMatch(line))
            {
                throw new HttpException(
                    HttpStatusCode.BadRequest,
                    string.Format(badRequest, line)
                );
            }

            return line;
        }

        private static string Trimmed(string value, HttpToken httpToken)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(
                    $"There is no token {httpToken.ToString()} in Request-Line header"
                );
            }

            return new Trimmed(value).AsString();
        }
    }
}