using System.Text;
using System.Text.Json.Nodes;
using Web.Elements.Bytes;
using Yaapii.Atoms.Bytes;

namespace Web.Elements.Auth;

public sealed class JwtPayload : IToken
{
    private readonly IIdentity identity;
    private readonly TimeSpan age;
    private readonly DateTime now;
    
    public const string issued = "iat";
    public const string expiration = "exp";

    public JwtPayload(IIdentity identity, long seconds)
        : this(identity, TimeSpan.FromSeconds(seconds))
    {
    }
    
    public JwtPayload(IIdentity identity, TimeSpan age)
    {
        this.identity = identity;
        this.age = age;
        this.now = DateTime.UtcNow;
    }

    public byte[] Encoded()
    {
        return new BytesBase64Url(
            new BytesOf(
                Json().ToJsonString(),
                Encoding.UTF8
            )
        ).AsBytes();
    }

    public JsonObject Json()
    {
        var expiration = DateTime.UtcNow.Add(age);

        var node = new JsonObject
        {
            { issued, this.now.Ticks },
            { JwtPayload.expiration, expiration.Ticks },
            { IdentityUser.PropertyType.identifier, identity.Identifier() }
        };

        foreach (var property in identity.Properties())
        {
            if (!node.ContainsKey(property.Key))
            {
                node.Add(property.Key, property.Value);
            }
        }

        return node;
    }
}