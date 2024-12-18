using System.Net;
using System.Text;

namespace Web.Elements.Rs;

public sealed class  RsWithCookie : RsWrap
{
    private const string setCookie = "Set-Cookie";

    public RsWithCookie(IResponse origin, string name, string value, params string[] attrs) :
        base(
            new RsWithHeader(
                origin,
                setCookie,
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