using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Text
{
    public class FirstSegment : IText
    {
        private readonly IScalar<string> _text;

        public FirstSegment(string text, char separator)
        {
            _text = new ScalarOf<string>(() =>
            {
                int index = text.IndexOf(separator);
                return index != -1 ? text.Substring(0, index) : text;
            });
        }

        public string AsString()
        {
            return _text.Value();
        }
    }
}
