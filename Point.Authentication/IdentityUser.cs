using Point.Authentication.Interfaces;

namespace Point.Authentication;

public class IdentityUser : IIdentity
{
    private readonly string _identifier;
    private readonly IDictionary<string, string> _data;

    public IdentityUser(string identifier)
       : this(identifier, new Dictionary<string, string>())
    {
        
    }
    
    public IdentityUser(string identifier, IDictionary<string, string> data)
    {
        _identifier = identifier;
        _data = data;
    }

    public string Identifier()
    {
        return _identifier;
    }

    public IDictionary<string, string> Data()
    {
        return _data;
    }
}