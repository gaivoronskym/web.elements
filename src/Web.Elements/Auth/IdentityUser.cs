using System.Text.Json.Nodes;

namespace Web.Elements.Auth;

public sealed class IdentityUser : IIdentity
{
    private readonly string identifier;
    private readonly IDictionary<string, string> data;

    public IdentityUser(JsonObject json)
        : this(
            json[PropertyType.identifier]!.ToString(),
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
        this.identifier = identifier;

        if (data.ContainsKey(IdentityUser.PropertyType.identifier))
        {
            data.Remove(IdentityUser.PropertyType.identifier);
        }

        this.data = data;
    }

    public string Identifier()
    {
        return identifier;
    }

    public IDictionary<string, string> Properties()
    {
        return data;
    }

    public class PropertyType
    {
        public const string identifier = "Identifier";
        public const string username = "Username";
        public const string email = "Email";
        public const string phone = "Phone";
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