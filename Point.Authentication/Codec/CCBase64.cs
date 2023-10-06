using Point.Authentication.Interfaces;
using Yaapii.Atoms.Bytes;

namespace Point.Authentication.Codec
{
    public class CCBase64 : ICodec
    {
        private readonly ICodec _origin;

        public CCBase64(ICodec origin)
        {
            _origin = origin;
        }

        public byte[] Encode(IIdentity identity)
        {
            return new BytesBase64(
                    new BytesOf(_origin.Encode(identity))
                ).AsBytes();
        }

        public IIdentity Decode(byte[] data)
        {
            return _origin.Decode(
               new Base64Bytes(
                    new BytesOf(
                              data
                         )
                   ).AsBytes()
            );
        }
    }
}
