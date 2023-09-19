using System.Text.Json.Nodes;
using Point;
using Point.Authentication;
using Point.Pt;
using Point.Rq;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public sealed class PtLogin : IPoint
{
    public IResponse Act(IRequest req)
    {
        /*var multipart = new RqMultipart(req);
        var part = multipart.Part("image").First();
        
        using var fileStream = File.Create("default.png");
        part.Body().CopyTo(fileStream);*/
        
        /*StreamReader reader = new StreamReader(part.Body());
        
        var temp = reader.ReadToEnd();
        */
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