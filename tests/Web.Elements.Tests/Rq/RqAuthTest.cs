using Web.Elements.Rq;

namespace Web.Elements.Tests.Rq;

public sealed class RqAuthTest
{
    [Fact]
    public void GetIdentity()
    {
        var req = new RqAuth(
            new RqWithHeader(
                new RqFake(),
                "Identity: Identifier=3"
            ),
            "Identity"
        );

        var id = req.Identity().Identifier();
        
        Assert.Equal("3", id);
    }
}