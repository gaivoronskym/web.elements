using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Web.Elements.Codec;

public class CcHex : ICodec
{
    private readonly ICodec origin;

    public CcHex(ICodec origin)
    {
        this.origin = origin;
    }

    public byte[] Encode(IIdentity identity)
    {
        return new BytesOf(
            new HexOf(
                new BytesOf(
                    origin.Encode(identity)
                )
            )
        ).AsBytes();
    }

    public IIdentity Decode(byte[] data)
    {
        return origin.Decode(
            new HexBytes(
                new TextOf(data)
            ).AsBytes()
        );
    }
}