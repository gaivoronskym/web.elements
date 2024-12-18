using Web.Elements.Auth;
using Web.Elements.Codec;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Codec;

public class CcHexTest
{
    [Fact]
    public void EncodeIdentity()
    {
        var userid = Guid.NewGuid().ToString();

        Assert.True(
            new CcHex(
                new CcPlain()
            ).Encode(
                new IdentityUser(
                    userid
                )
            ).SequenceEqual(
                new BytesOf(
                    new HexOf(
                        new BytesOf(
                            new Formatted(
                                "{0}={1}",
                                IdentityUser.PropertyType.identifier,
                                userid
                            )
                        )
                    )
                ).AsBytes()
            )
        );
    }


    [Fact]
    public void DecodeIdentity()
    {
        var userid = Guid.NewGuid().ToString();
        var identity = new CcHex(
            new CcPlain()
        ).Decode(
            new BytesOf(
                new HexOf(
                    new BytesOf(
                        new Formatted(
                            "{0}={1}",
                            IdentityUser.PropertyType.identifier,
                            userid
                        )
                    )
                )
            ).AsBytes()
        );
        
        Assert.Equal(userid, identity.Identifier());
    }
}