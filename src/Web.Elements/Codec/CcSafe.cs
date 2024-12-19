using Web.Elements.Auth;

namespace Web.Elements.Codec;

public class CcSafe : ICodec
{
    private readonly ICodec origin;

    public CcSafe(ICodec origin)
    {
        this.origin = origin;
    }

    public byte[] Encode(IIdentity identity)
    {
        return origin.Encode(identity);
    }

    public IIdentity Decode(byte[] data)
    {
        try
        {
            var identity = origin.Decode(data);
            return identity;
        }
        catch (Exception)
        {
            return new Anonymous();
        }
    }
}