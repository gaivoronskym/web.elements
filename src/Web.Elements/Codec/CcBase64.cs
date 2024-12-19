using Web.Elements.Bytes;
using Yaapii.Atoms.Bytes;

namespace Web.Elements.Codec;

public class CcBase64 : ICodec
{
    private readonly ICodec origin;

    public CcBase64(ICodec origin)
    {
        this.origin = origin;
    }

    public byte[] Encode(IIdentity identity)
    {
        return new BytesBase64Url(
            new BytesOf(
                origin.Encode(
                    identity
                )
            )
        ).AsBytes();
    }

    public IIdentity Decode(byte[] data)
    {
        return origin.Decode(
            new Base64UrlBytes(
                new BytesOf(
                    data
                )
            ).AsBytes()
        );
    }
}