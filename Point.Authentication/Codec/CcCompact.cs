using Point.Authentication.Interfaces;

namespace Point.Authentication.Codec;

public class CcCompact : ICodec
{
    public byte[] Encode(IIdentity identity)
    {
        throw new NotImplementedException();
    }

    public IIdentity Decode(byte[] data)
    {
        throw new NotImplementedException();
    }
}