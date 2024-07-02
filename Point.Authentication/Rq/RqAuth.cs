using Point.Authentication.Codec;
using Point.Authentication.Interfaces;
using Point.Authentication.Rq.Interfaces;
using Point.Rq;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Point.Authentication.Rq
{
    public class RqAuth : RqWrap, IRqAuth
    {
        private readonly IRequest _origin;
        private readonly string _header;

        public RqAuth(IRequest origin, string header) : base(origin)
        {
            _origin = origin;
            _header = header;
        }

        public IIdentity Identity()
        {
            var headers = new RqHeaders(_origin).Headers();
            if (headers.ContainsKey(_header))
            {
                var value = headers[_header];
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
