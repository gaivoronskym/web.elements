using Point.Authentication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Point.Authentication.Codec
{
    public class CCPlain : ICodec
    {
        public byte[] Encode(IIdentity identity)
        {
            StringBuilder text = new StringBuilder();
            text.Append(typeof(IdentityUser).Name)
                .Append("=")
                .Append(identity.Identifier());

            foreach (KeyValuePair<string, string> item in identity.Properties())
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
            IList<string> parts = new ListOf<string>(
                    new Split(
                    new TextOf(data),
                    ";"
                )
            );

            IDictionary<string, string> map = new Dictionary<string, string>(parts.Count);

            foreach (string item in parts)
            {
                IList<string> pair = new ListOf<string>(
                      new Split(item, "=")
                );

                map.Add(pair.First(), pair.Last());
            }

            return new IdentityUser(
                    map.First().Value,
                    map
            );
        }
    }
}
