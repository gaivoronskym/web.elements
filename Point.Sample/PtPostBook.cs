﻿using System.Text.Json.Nodes;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;

namespace Point.Sample;

public sealed class PtPostBook : IPtRegex
{
    public Task<IResponse> Act(IRqRegex req)
    {
        var text = new TextOf(
            new InputOf(req.Body)
        ).AsString();

        return Task.FromResult<IResponse>(new RsJson(
                new JsonObject
                {
                    { "id", 1 }
                }
            )
        );
    }
}