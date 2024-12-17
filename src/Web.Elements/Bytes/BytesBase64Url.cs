using System.Text;
using Web.Elements.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Web.Elements.Bytes;

public class BytesBase64Url : IBytes
{
    private readonly IScalar<byte[]> bytes;

    public BytesBase64Url(byte[] bytes)
        : this(new BytesOf(bytes))
    {

    }

    public BytesBase64Url(IBytes bytes)
    {
        this.bytes = new ScalarOf<byte[]>(() =>
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
        return bytes.Value();
    }
}