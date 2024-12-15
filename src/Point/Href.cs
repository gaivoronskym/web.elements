using System.Collections;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Exception = System.Exception;

namespace Point;

public sealed class Href : IEnumerable<char>
{
    private readonly Uri uri;
    private readonly IDictionary<string, List<string>> queryParams;
    private readonly IOpt<string> fragment;
    
    private static Regex Slash = new Regex(@"/$");

    public Href()
        : this("about:blank")
    {
    }
    
    public Href(string uri)
        : this(new Uri(uri))
    {
    }
    
    public Href(Uri link)
        : this(Href.CreateBare(link), Href.AsMap(link.Query), Href.ReadFragment(link))
    {
    }

    public Href(Uri uri, IDictionary<string, List<string>> queryParams, IOpt<string> fragment)
    {
        this.uri = uri;
        this.queryParams = queryParams;
        this.fragment = fragment;
    }
    
    public string Path()
    {
        return this.uri.PathAndQuery;
    }

    public string LocalPath()
    {
        return this.uri.LocalPath;
    }

    public string Bare()
    {
        return this.uri.AbsoluteUri;
    }

    public override string ToString()
    {
        StringBuilder text = new StringBuilder(this.Bare());

        if (this.queryParams.Count > 0)
        {
            bool first = true;
            foreach (var (key, values) in this.queryParams)
            {
                foreach (var value in values)
                {
                    if (first)
                    {
                        text.Append("?");
                        first = false;
                    }
                    else
                    {
                        text.Append("&");
                    }
                    text.Append(Href.Encode(key));
                    if (!string.IsNullOrEmpty(value))
                    {
                        text.Append("=");
                        text.Append(Href.Encode(value));
                    }
                }
            }
            
        }

        if (this.fragment.Has())
        {
            text.Append("#")
                .Append(this.fragment.Value());
        }
        
        return text.ToString();
    }

    public IList<string> Param(string name)
    {
        IList<string> res;

        if (this.queryParams.ContainsKey(name))
        {
            res = this.queryParams[name];
        }
        else
        {
            res = new List<string>();
        }

        return res;
    }

    public Href With(string key, string value)
    {
        IDictionary<string, List<string>> map = new Dictionary<string, List<string>>(this.queryParams);

        if (map.ContainsKey(key))
        {
            map[key].Add(value);
        }
        else
        {
            map.Add(key, new List<string> { value });
        }
        
        return new Href(this.uri, map, this.fragment);
    }

    public Href Path(string suffix)
    {
        var path = new StringBuilder(
            Slash.Replace(this.uri.ToString(), string.Empty)
        ).Append("/")
        .Append(Href.Encode(suffix))
        .ToString();
        
        return new Href(new Uri(path), this.queryParams, this.fragment);
    }

    public Href Without(string key)
    {
        IDictionary<string, List<string>> map = new Dictionary<string, List<string>>(this.queryParams);
        
        if (map.ContainsKey(key))
        {
            map.Remove(key);
        }
        
        return new Href(this.uri, map, this.fragment);
    }
    
    public IEnumerator<char> GetEnumerator()
    {
        return this.ToString()!.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private static string Encode(string text)
    {
        try
        {
            return WebUtility.UrlEncode(text);
        }
        catch (Exception)
        {
            throw new UriFormatException($"Failed to decode {text}");
        }
    }

    private static string Decode(string text)
    {
        try
        {
            return WebUtility.UrlDecode(text);
        }
        catch (Exception)
        {
            throw new UriFormatException($"Failed to decode {text}");
        }
    }

    private static IDictionary<string, List<string>> AsMap(string query)
    {
        
        IDictionary<string, List<string>> map = new Dictionary<string, List<string>>();
        if (!string.IsNullOrEmpty(query))
        {
            var formatted = query.StartsWith("?") ? new string(query.Skip(1).ToArray()) : query;
            foreach (var pair in formatted.Split('&'))
            {
                var parts = pair.Split('=');
                var key = parts[0];
                string value;
                if (parts.Length > 1)
                {
                    value = Decode(parts[1]);
                }
                else
                {
                    value = string.Empty;
                }

                if (map.ContainsKey(key))
                {
                    map[key].Add(value);
                }
                else
                {
                    map.Add(key, new List<string> { value });
                }
            }

            return map;
        }
        else
        {
            return map;
        }
    }
    
    private static Uri CreateBare(Uri link)
    {
        Uri uri;

        if (string.IsNullOrEmpty(link.Query) && string.IsNullOrEmpty(link.Fragment))
        {
            uri = link;
        }
        else
        {
            var href = link.ToString();
            int index;
            if (string.IsNullOrEmpty(link.Query))
            {
                index = href.IndexOf('#');
            }
            else
            {
                index = href.IndexOf('?');
            }
            
            uri = new Uri(href.Substring(0, index));
        }

        return uri;
    }

    private static IOpt<string> ReadFragment(Uri link)
    {
        IOpt<string> fragment;
        if (string.IsNullOrEmpty(link.Fragment))
        {
            fragment = new IOpt<string>.Empty();
        }
        else
        {
            fragment = new Opt<string>(link.Fragment);
        }

        return fragment;
    }
}