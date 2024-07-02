using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Bytes
{
    public class Base64UrlBytes : IBytes
    {
        private readonly IScalar<byte[]> bytes;

        public Base64UrlBytes(byte[] bytes)
            : this(new BytesOf(bytes))
        {

        }

        public Base64UrlBytes(IBytes bytes)
        {
            this.bytes = new ScalarOf<byte[]>(() =>
            {
                var output = new TextOf(bytes).AsString();
                output = new Replaced(
                    new Replaced(
                        new TextOf(output),
                        "-",
                        "+"
                    ),
                    "_",
                    "/"
                ).AsString();

                switch (output.Length % 4) // Pad with trailing '='s
                {
                    case 0:
                        break; // No pad chars in this case
                    case 2:
                        output += "==";
                        break; // Two pad chars
                    case 3:
                        output += "=";
                        break; // One pad char
                    default:
                        throw new FormatException("Illegal base64url string.");
                }

                return Convert.FromBase64String(output);
            });
        }

        public byte[] AsBytes()
        {
            return bytes.Value();
        }
    }
}
