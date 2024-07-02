﻿using Point.Authentication.Fk;
using Point.Fk;
using Point.Pt;

namespace CustomServer;

public sealed class PtAuthenticated : PtWrap
{
    public PtAuthenticated(IPoint origin)
        : base(
            new PtFork(
                new FkAuthenticated(
                    origin,
                    "Authorization"
                )
            )
        )
    {
    }
}