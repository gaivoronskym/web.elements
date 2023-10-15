using Point.Authentication.Interfaces;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace Point.Authentication;

public sealed class IdentityUser : IIdentity
{
    private readonly string _identifier;
    private readonly IDictionary<string, string> _data;

    public IdentityUser(JsonObject json)
        : this(
              json[typeof(IdentityUser).Name]!.ToString(),
              () =>
              {
                  IDictionary<string, string> map = new Dictionary<string, string>();

                  foreach (var item in json.ToArray())
                  {
                      map.Add(item.Key, item.Value!.ToString());
                  }

                  return map;
              })
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

        if (data.ContainsKey(typeof(IdentityUser).Name))
        {
            data.Remove(typeof(IdentityUser).Name);
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
}