using Point.Authentication.Codec;
using Point.Authentication.Interfaces;
using Point.Authentication.Pt;
using Point.Authentication.Rq.Interfaces;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Point.Authentication.Rq
{
    public class RqAuth : RqWrap, IRqAuth
    {
        private readonly IRequest origin;
        private readonly string header;

        public RqAuth(IRequest origin)
            : this(origin, nameof(PtAuth))
        {
        }
        
        public RqAuth(IRequest origin, string header) : base(origin)
        {
            this.origin = origin;
            this.header = header;
        }

        public IIdentity Identity()
        {
            var headers = new RqHeaders(origin).Headers();
            if (headers.ContainsKey(header))
            {
                var value = headers[header];
                return new CcPlain().Decode(
                    new BytesOf(
                        new TextOf(value)
                    ).AsBytes()
                );
            }

            return new Anonymous();
        }
    }
}
