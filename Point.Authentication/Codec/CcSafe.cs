using Point.Authentication.Interfaces;

namespace Point.Authentication.Codec;

public class CcSafe : ICodec
{
    private readonly ICodec _origin;

    public CcSafe(ICodec origin)
    {
        _origin = origin;
    }

    public byte[] Encode(IIdentity identity)
    {
        return _origin.Encode(identity);
    }

    public IIdentity Decode(byte[] data)
    {
        try
        {
            IIdentity identity = _origin.Decode(data);
            return identity;
        }
        catch (Exception e)
        {
            return new Anonymous();
        }
    }
}