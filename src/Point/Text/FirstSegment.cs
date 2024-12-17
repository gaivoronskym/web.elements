using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Text;

public class FirstSegment : IText
{
    private readonly IScalar<string> text;

    public FirstSegment(string text, char separator)
    {
        this.text = new ScalarOf<string>(() =>
        {
            var index = text.IndexOf(separator);
            return index != -1 ? text.Substring(0, index) : text;
        });
    }

    public string AsString()
    {
        return text.Value();
    }
}