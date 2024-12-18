using Yaapii.Atoms.Text;

namespace Web.Elements.Rq;

public class RqCookies : IRqCookies
{
    private readonly IRequest origin;

    public RqCookies(IRequest origin)
    {
        this.origin = origin;
    }

    public IEnumerable<string> Head()
    {
        return origin.Head();
    }

    public Stream Body()
    {
        return origin.Body();
    }

    public string Cookie(string key)
    {
        var defaultPair = new KeyValuePair<string, string>(string.Empty, string.Empty);
        return Map().FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase), defaultPair).Value;
    }

    public IEnumerable<string> Names()
    {
        return Map().Keys;
    }

    private IDictionary<string, string> Map()
    {
        IDictionary<string, string> map = new Dictionary<string, string>();
        var values = new RqHeaders(origin).Header("Cookie");
        foreach (var item in values)
        {
            foreach (var pair in item.Split(";"))
            {
                var parts = pair.Split("=", 2);
                var key = new Trimmed(parts[0]).AsString();
                if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
                {
                    map.Add(key,
                        new Trimmed(parts[1]).AsString()
                    );
                }
                else
                {
                    map.Remove(key);
                }
            }
        }

        return map;
    }
}