using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Web.Elements.Codec;

public class CcXor : ICodec
{
    private readonly ICodec origin;
    private readonly IBytes secret;


    public CcXor(ICodec origin, string secret)
        : this(origin, new BytesOf(new TextOf(secret)))
    {
    }
    
    public CcXor(ICodec origin, byte[] secret)
        : this(origin, new BytesOf(secret))
    {
    }
    
    public CcXor(ICodec origin, IBytes secret)
    {
        this.origin = origin;
        this.secret = secret;
    }


    public byte[] Encode(IIdentity identity)
    {
        return Xor(origin.Encode(identity));
    }


    public IIdentity Decode(byte[] data)
    {
        return origin.Decode(Xor(data));
    }
    
    private byte[] Xor(byte[] input)
    {
        if (input.Length == 0)
        {
            var temp = new Span<byte>();
            input.CopyTo(temp);
            return temp.ToArray();
        }

        var output = new byte[input.Length];

        var spos = 0;
        for (var pos = 0; pos < input.Length; ++pos)
        {
            output[pos] = (byte)(input[pos] ^ secret.AsBytes()[spos]);

            ++spos;
            if ( spos >= secret.AsBytes().Length )
            {
                spos = 0;
            }
        }

        return output;
    }
}