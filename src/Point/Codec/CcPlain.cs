using System.Text;
using Point.Auth;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Codec;

public class CcPlain : ICodec
{
    public byte[] Encode(IIdentity identity)
    {
        var text = new StringBuilder();
        text.Append(IdentityUser.PropertyType.Identifier)
            .Append("=")
            .Append(identity.Identifier());

        foreach (var item in identity.Properties())
        {
            text.Append(";")
                .Append(item.Key)
                .Append("=")
                .Append(item.Value);
        }

        return new BytesOf(
            new TextOf(text.ToString())
        ).AsBytes();
    }

    public IIdentity Decode(byte[] data)
    {
        try
        {
            IList<string> parts = new ListOf<string>(
                new Split(
                    new TextOf(data),
                    ";"
                )
            );

            IDictionary<string, string> map = new Dictionary<string, string>(parts.Count);

            foreach (var item in parts)
            {
                IList<string> pair = new ListOf<string>(
                    new Split(item, "=")
                );
                var key = pair.First();
                if (map.ContainsKey(key))
                    continue;

                map.Add(key, pair.Last());
            }

            return new IdentityUser(
                map.First().Value,
                map
            );
        }
        catch (Exception)
        {
            return new Anonymous();
        }
    }
}