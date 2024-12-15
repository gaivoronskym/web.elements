using Point.Auth;
using Point.Bytes;
using Yaapii.Atoms.Bytes;

namespace Point.Tests.Auth;

public class TokenTest
{
    [Fact]
    public void HeaderAlgorithm()
    {
        var header = new JwtHeader("SHA256").Json();
        
        Assert.Equal("SHA256", header["alg"].ToString());
    }

    [Fact]
    public void HeaderEncoded()
    {
        var header = new JwtHeader("SHA256");
        byte[] code = header.Encoded();
        var expected = new BytesBase64Url(
            new BytesOf(
                "{\"alg\":\"SHA256\",\"typ\":\"JWT\"}"
            )
        ).AsBytes();
        
        Assert.True(code.SequenceEqual(expected));
    }

    [Fact]
    public void JwtExpiration()
    {
        var body = new JwtPayload(
            new IdentityUser("user"),
            3600
        ).Json();

        Assert.NotEqual(
            body[JwtPayload.Issued]!.ToString(),
            body[JwtPayload.Expiration]!.ToString()
        );
        
        var issued = new DateTime(long.Parse(body[JwtPayload.Issued]!.ToString()), DateTimeKind.Utc);
        var expiration = new DateTime(long.Parse(body[JwtPayload.Expiration]!.ToString()), DateTimeKind.Utc);
        
        Assert.True(issued < expiration);
    }
}