﻿using Yaapii.Atoms;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Rq.Interfaces;

public interface IRqHref : IRequest
{
    Href Href();
    
    public sealed class Base : IRqHref
    {
        private readonly IRequest origin;
        private const string HeaderDelimiter = ": ";

        public Base(IRequest origin)
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

        public Href Href()
        {
            var uri = new IRqRequestLine.Base(this).Uri();
            var hosts = new IRqHeaders.Base(this).Header("Host").ToList();
            var protos = new IRqHeaders.Base(this).Header("x-forwarded-proto").ToList();

            IText host = hosts.Any() ? new Trimmed(new TextOf(hosts[0])) : new TextOf("localhost");
            IText proto = protos.Any() ? new Trimmed(new TextOf(protos[0])) : new TextOf("http");

            return new Href(
                $"{proto.AsString()}://{host.AsString()}{uri}"
            );
        }
    }
}