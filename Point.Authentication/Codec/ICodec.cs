using Point.Authentication.Interfaces;

namespace Point.Authentication.Codec
{
    public interface ICodec
    {
        byte[] Encode(IIdentity identity);

        IIdentity Decode(byte[] data);
    }
}
