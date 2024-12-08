using Yaapii.Atoms;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Rq.Interfaces;

public interface IRqUri : IRequest
{
    Uri Uri();

    IDictionary<string, string> Query();

    public sealed class Base : IRqUri
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

        public Uri Uri()
        {
            var uri = new IRqRequestLine.Base(this).Uri();
            var hosts = new IRqHeaders.Base(this).Header("Host").ToList();
            var protos = new IRqHeaders.Base(this).Header("x-forwarded-proto").ToList();

            IText host = hosts.Any() ? new Trimmed(new TextOf(hosts[0])) : new TextOf("localhost");
            IText proto = protos.Any() ? new Trimmed(new TextOf(protos[0])) : new TextOf("http");

            return new Uri(
                $"{proto.AsString()}://{host.AsString()}{uri}"
            );
        }

        public IDictionary<string, string> Query()
        {
            var query = Uri().Query;

            if (string.IsNullOrEmpty(query))
            {
                return new Dictionary<string, string>();
            }

            query = query.TrimStart('?');

            var list = new ListOf<string>(
                new Split(query, "&")
            );

            var map = new Dictionary<string, string>();
            foreach (var queryParam in list)
            {
                var splittedParam = new Split(
                    queryParam,
                    "="
                );
                var key = splittedParam.First();
                var value = splittedParam.Last();
                map.Add(key, value);
            }

            return map;
        }
    }
}