using Web.Elements.Auth;
using Web.Elements.Codec;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Codec;

public class CcPlainTest
{
    [Fact]
    public void EncodeIdentityWithoutProperties()
    {
        var userid = Guid.NewGuid().ToString();

        Assert.Equal(
            actual: new CcPlain()
                .Encode(
                    new IdentityUser(
                        userid
                    )
                ),
            expected: new BytesOf(
                new Formatted(
                    "{0}={1}",
                    IdentityUser.PropertyType.identifier,
                    userid
                )
            ).AsBytes()
        );
    }

    [Fact]
    public void EncodeIdentityWithProperties()
    {
        var userid = Guid.NewGuid().ToString();

        Assert.Equal(
            actual: new CcPlain()
                .Encode(
                    new IdentityUser(
                        userid,
                        new MapOf<string>(
                            new KvpOf<string>(IdentityUser.PropertyType.email, "test@gmail.com"),
                            new KvpOf<string>(IdentityUser.PropertyType.username, "test")
                        )
                    )
                ),
            expected: new BytesOf(
                new Formatted(
                    "{0}={1};{2}={3};{4}={5}",
                    IdentityUser.PropertyType.identifier,
                    userid,
                    IdentityUser.PropertyType.email,
                    "test@gmail.com",
                    IdentityUser.PropertyType.username,
                    "test"
                )
            ).AsBytes()
        );
    }

    [Fact]
    public void DecodeIdentity()
    {
        var userid = Guid.NewGuid().ToString();
        var identity = new CcPlain()
            .Decode(
                new BytesOf(
                    new Formatted(
                        "{0}={1};{2}={3};{4}={5}",
                        IdentityUser.PropertyType.identifier,
                        userid,
                        IdentityUser.PropertyType.email,
                        "test@gmail.com",
                        IdentityUser.PropertyType.username,
                        "test"
                    )
                ).AsBytes()
            );
        
        Assert.Equal(userid, identity.Identifier());
    }
}