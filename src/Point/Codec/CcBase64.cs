using Point.Bytes;
using Yaapii.Atoms.Bytes;

namespace Point.Codec;

public class CcBase64 : ICodec
{
    private readonly ICodec _origin;

    public CcBase64(ICodec origin)
    {
        _origin = origin;
    }

    public byte[] Encode(IIdentity identity)
    {
        return new BytesBase64Url(
            new BytesOf(
                _origin.Encode(
                    identity
                )
            )
        ).AsBytes();
    }

    public IIdentity Decode(byte[] data)
    {
        return _origin.Decode(
            new Base64UrlBytes(
                new BytesOf(
                    data
                )
            ).AsBytes()
        );
    }
}