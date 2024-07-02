using Point.Text;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Point.Bytes
{
    public class BytesBase64Url : IBytes
    {
        private readonly IScalar<byte[]> _bytes;

        public BytesBase64Url(byte[] bytes)
            : this(new BytesOf(bytes))
        {

        }

        public BytesBase64Url(IBytes bytes)
        {
            _bytes = new ScalarOf<byte[]>(() =>
            {
                var base64 = Convert.ToBase64String(bytes.AsBytes());
                var output = new FirstSegment(base64, '=').AsString();

                return Encoding.UTF8.GetBytes(
                    new Replaced(
                      new Replaced(
                        new TextOf(output),
                        "+",
                        "-"
                      ),
                        "/",
                        "_"
                     ).AsString()
                   );
            });
        }

        public byte[] AsBytes()
        {
            return _bytes.Value();
        }
    }
}
