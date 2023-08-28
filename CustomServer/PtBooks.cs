﻿using System.Text.Json.Nodes;
using Point;
using Point.Pt;
using Point.Rs;

namespace CustomServer;

public class PtBooks : IPoint
{
    public IResponse Act(IRequest req)
    {

        return new RsJson(
            new JsonArray(new JsonObject
                {
                    { "Title", "Object thinking" }
                }
            )
        );
    }
}