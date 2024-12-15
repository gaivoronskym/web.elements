using Point.Auth;

namespace Point.Codec;

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
            var identity = _origin.Decode(data);
            return identity;
        }
        catch (Exception)
        {
            return new Anonymous();
        }
    }
}