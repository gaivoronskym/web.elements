using Point.Authentication.Interfaces;

namespace Point.Authentication;

public sealed class Anonymous : IIdentity
{
    public string Identifier()
    {
        return string.Empty;
    }

    public IDictionary<string, string> Data()
    {
        return new Dictionary<string, string>();
    }
}