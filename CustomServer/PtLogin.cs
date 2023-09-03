using System.Globalization;
using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public class PtLogin : IPoint
{
    public IResponse Act(IRequest req)
    {
        string cookieDate = DateTime.UtcNow.AddMinutes(60).ToString("ddd, dd-MMM-yyyy H:mm:ss");

        return new RsWithCookie(
            new RsJson(new JsonObject()),
            "userId", "1231312",
            "Path=/", $"Expires={cookieDate}"
        );
    }
}