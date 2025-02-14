﻿using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rs;

public class RsTextTest
{
    [Fact]
    public void MakesTextPlainResponse()
    {
        var body = "Hello, world";

        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                $"Content-Length: {body.Length}",
                "Content-Type: text/plain",
                "",
                body
            ).AsString(),
            new RsPrint(
                new RsText(
                    body
                )
            ).AsString()
        );
    }
}