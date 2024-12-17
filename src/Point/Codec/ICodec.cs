namespace Point.Codec;

public interface ICodec
{
    byte[] Encode(IIdentity identity);

    IIdentity Decode(byte[] data);
}