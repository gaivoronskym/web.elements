using System.Globalization;
using System.Text.Json.Nodes;
using Point;
using Point.Authentication;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public class PtLogin : IPoint
{
    public IResponse Act(IRequest req)
    {
        var obj = JsonNode.Parse(req.Body());
        var jsonString = obj.ToJsonString();
        
        // string cookieDate = DateTime.UtcNow.AddMinutes(60).ToString("ddd, dd-MMM-yyyy H:mm:ss");

        var jwtToken = new JwtToken(
            new IdentityUser("12345"),
            "Server",
            "https://localhost",
            "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH",
            44640
        );
        
        return new RsJson(new JsonObject
        {
            {"token", jwtToken.AsString()}
        });
    }
}