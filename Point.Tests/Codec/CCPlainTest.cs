using Point.Authentication;
using Point.Authentication.Codec;
using System.Security.Claims;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Text;

namespace Point.Tests.Codec
{
    public class CCPlainTest
    {
        [Fact]
        public void EncodeIdentityWithoutProperties()
        {
            string userid = Guid.NewGuid().ToString();

            Assert.Equal(
                    actual: new CCPlain()
                        .Encode(
                            new IdentityUser(
                                userid
                            )
                        ),
                    expected: new BytesOf(
                        new Formatted(
                            "{0}={1}",
                            ClaimTypes.NameIdentifier,
                            userid
                           )
                       ).AsBytes()
                );
        }

        [Fact]
        public void EncodeIdentityWithProperties()
        {
            string userid = Guid.NewGuid().ToString();

            Assert.Equal(
                    actual: new CCPlain()
                        .Encode(
                            new IdentityUser(
                                userid,
                                new MapOf<string>(
                                        new KvpOf<string>(ClaimTypes.Email, "test@gmail.com"),
                                        new KvpOf<string>(ClaimTypes.Name, "test")
                                    )
                             )
                        ),
                    expected: new BytesOf(
                        new Formatted(
                            "{0}={1};{2}={3};{4}={5}",
                            ClaimTypes.NameIdentifier,
                            userid,
                            ClaimTypes.Email,
                            "test@gmail.com",
                            ClaimTypes.Name,
                            "test"
                           )
                       ).AsBytes()
                );
        }

        [Fact]
        public void DecodeIdentity()
        {
            string userid = Guid.NewGuid().ToString();
            new CCPlain()
                .Decode(
                    new BytesOf(
                        new Formatted(
                            "{0}={1};{2}={3};{4}={5}",
                            ClaimTypes.NameIdentifier,
                            userid,
                            ClaimTypes.Email,
                            "test@gmail.com",
                            ClaimTypes.Name,
                            "test"
                           )
                       ).AsBytes()
                );
        }
    }
}
