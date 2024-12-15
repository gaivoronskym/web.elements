namespace Point.Auth;

public sealed class Anonymous : IIdentity
{
    public string Identifier()
    {
        return string.Empty;
    }

    public IDictionary<string, string> Properties()
    {
        return new Dictionary<string, string>();
    }
}