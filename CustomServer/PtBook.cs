﻿using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rq;
using Point.Rs;

namespace CustomServer;

public class PtBook : IPoint
{
    public IResponse Act(IRequest req)
    {
        var uri = new RqUri(req).Uri();
        
        return new RsJson(new JsonObject
            {
                { "Title", "Object thinking" }
            }
        );
    }
}