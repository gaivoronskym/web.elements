using Point.Authentication.Interfaces;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Point.Authentication.Codec
{
    public class CcHex : ICodec
    {
        private readonly ICodec _origin;

        public CcHex(ICodec origin)
        {
            _origin = origin;
        }

        public byte[] Encode(IIdentity identity)
        {
            return new BytesOf(
                new HexOf(
                    new BytesOf(
                        _origin.Encode(identity)
                        )
                    )
                ).AsBytes();
        }

        public IIdentity Decode(byte[] data)
        {
            return _origin.Decode(
                new HexBytes(
                    new TextOf(data)
                ).AsBytes()
           );
        }
    }
}
