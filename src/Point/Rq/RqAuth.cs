using Point.Auth;
using Point.Codec;
using Point.Pt;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;

namespace Point.Rq
{
    public class RqAuth : RqWrap, IRqAuth
    {
        private readonly IRqHeaders origin;
        private readonly string header;

        public RqAuth(IRequest origin)
            : this(origin, nameof(PtAuth))
        {
        }
        
        public RqAuth(IRequest origin, string header) : base(origin)
        {
            this.origin = new IRqHeaders.Base(origin);
            this.header = header;
        }

        public IIdentity Identity()
        {
            var headers = this.origin.Header(header);
            if (headers.Any())
            {
                return new CcPlain().Decode(
                    new BytesOf(
                        new TextOf(header[0])
                    ).AsBytes()
                );
            }

            return new Anonymous();
        }
    }
}
