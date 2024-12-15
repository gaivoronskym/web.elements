using System.Net;
using System.Text;

namespace Point.Rs;

public sealed class  RsWithCookie : RsWrap
{
    private const string SetCookie = "Set-Cookie";

    public RsWithCookie(IResponse origin, string name, string value, params string[] attrs) :
        base(
            new RsWithHeader(
                origin,
                SetCookie,
                Join(name, value, attrs)
            )
        )
    {
    }

    private static string Join(string name, string value, params string[] attrs)
    {
        var text = new StringBuilder(
            $"{name}={value};"
        );

        foreach (var attr in attrs)
        {
            text.Append(attr).Append(";");
        }

        return text.ToString();
    }
}