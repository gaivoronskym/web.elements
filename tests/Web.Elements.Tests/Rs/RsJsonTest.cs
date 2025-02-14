﻿using System.Text.Json.Nodes;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rs;

public class RsJsonTest
{
    [Fact]
    public void MakesJsonResponse()
    {
        var json = new JsonObject
        {
            { "Username", "Michael" }
        }.ToJsonString();

        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                $"Content-Length: {json.Length}",
                "Content-Type: application/json",
                "",
                json
            ).AsString(),
            new RsPrint(
                new RsJson(
                    json
                )
            ).AsString()
        );
    }
}