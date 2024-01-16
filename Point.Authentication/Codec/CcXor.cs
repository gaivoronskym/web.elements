using Point.Authentication.Interfaces;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Point.Authentication.Codec;

public class CcXor : ICodec
{
    private readonly ICodec _origin;
    private readonly byte[] _secret;

    public CcXor(ICodec origin, string secret)
         : this(origin, new BytesOf(new TextOf(secret)).AsBytes())
    { }
    
    public CcXor(ICodec origin, byte[] secret)
    {
        _origin = origin;
        _secret = secret;
    }

    public byte[] Encode(IIdentity identity)
    {
        return Xor(_origin.Encode(identity));
    }

    public IIdentity Decode(byte[] data)
    {
        return _origin.Decode(Xor(data));
    }

    private byte[] Xor(byte[] input)
    {
        if (input.Length == 0)
        {
            Span<byte> temp = new Span<byte>();
            input.CopyTo(temp);
            return temp.ToArray();
        }
        
        byte[] output = new byte[input.Length];

        int spos = 0;
        for (int pos = 0; pos < input.Length; ++pos)
        {
            output[pos] = (byte)(input[pos] ^ _secret[spos]);

            ++spos;
            if (spos >= _secret.Length)
            {
                spos = 0;
            }
        }

        return output;
    }
}