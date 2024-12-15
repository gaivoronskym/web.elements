using Point.Auth;
using Point.Codec;
using Point.Pt;
using Point.Rq.Interfaces;
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
            var names = this.origin.Names();
            var hasIdentity = new Contains<string>(
                names,
                i => i.StartsWith(header)
            );
            
            if (hasIdentity.Value())
            {
                var value = this.origin.Header(header)[0];
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
