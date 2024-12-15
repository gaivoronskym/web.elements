using System.Text.Json.Nodes;

namespace Point.Auth;

public sealed class IdentityUser : IIdentity
{
    private readonly string _identifier;
    private readonly IDictionary<string, string> _data;

    public IdentityUser(JsonObject json)
        : this(
            json[PropertyType.Identifier]!.ToString(),
            Make(json)
        )
    {
    }

    public IdentityUser(string identifier, Func<IDictionary<string, string>> func)
        : this(identifier, func())
    {
    }

    public IdentityUser(string identifier)
        : this(identifier, new Dictionary<string, string>())
    {
    }

    public IdentityUser(string identifier, IDictionary<string, string> data)
    {
        _identifier = identifier;

        if (data.ContainsKey(IdentityUser.PropertyType.Identifier))
        {
            data.Remove(IdentityUser.PropertyType.Identifier);
        }

        _data = data;
    }

    public string Identifier()
    {
        return _identifier;
    }

    public IDictionary<string, string> Properties()
    {
        return _data;
    }

    public class PropertyType
    {
        public const string Identifier = "Identifier";
        public const string Username = "Username";
        public const string Email = "Email";
        public const string Phone = "Phone";
    }

    private static IDictionary<string, string> Make(JsonObject json)
    {
        IDictionary<string, string> map = new Dictionary<string, string>();

        foreach (var item in json.ToArray())
        {
            map.Add(item.Key, item.Value!.ToString());
        }

        return map;
    }
}