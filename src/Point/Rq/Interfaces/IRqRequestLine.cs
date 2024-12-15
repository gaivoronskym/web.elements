﻿using System.Net;
using System.Text.RegularExpressions;
using Point.Exceptions;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Point.Rq.Interfaces;

public interface IRqRequestLine : IRequest
{
    string Header();

    string Method();

    string Uri();

    string Version();

    public sealed class Base : RqWrap, IRqRequestLine
    {
        private static readonly Regex Pattern = new Regex("([!-~]+) ([^ ]+)( [^ ]+)?", RegexOptions.Compiled);

        private static string BadRequest = "Invalid HTTP Request-Line header: {0}";

        private enum HttpToken
        {
            Method = 1,
            
            URI = 2,
            
            Version = 3
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
            return this.Token(HttpToken.Method);
        }

        public string Uri()
        {
            return this.Token(HttpToken.URI);
        }

        public string Version()
        {
            return this.Token(HttpToken.Version);
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
            if (!Pattern.IsMatch(line))
            {
                throw new HttpException(
                    HttpStatusCode.BadRequest,
                    string.Format(BadRequest, line)
                );
            }
            
            return Pattern.Match(line);
        }

        private static string Validated(string line)
        {
            if (!Pattern.IsMatch(line))
            {
                throw new HttpException(
                    HttpStatusCode.BadRequest,
                    string.Format(BadRequest, line)
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