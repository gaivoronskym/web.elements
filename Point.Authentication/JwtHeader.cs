using Point.Authentication.Interfaces;
using System.Text.Json.Nodes;
using Yaapii.Atoms.Bytes;

namespace Point.Authentication
{
    public class JwtHeader : IToken
    {
        private readonly string _algorithm;

        private const string Algorithm = "alg";
        private const string Typ = "typ";

        public JwtHeader(string algorithm)
        {
            _algorithm = algorithm;
        }

        public byte[] Encoded()
        {
            return new BytesBase64(
                    new BytesOf(
                        Json().ToJsonString()
                     )
               ).AsBytes();
        }

        public JsonObject Json()
        {
            JsonObject node = new JsonObject();
            node.Add(Algorithm, _algorithm);
            node.Add(Typ, "JWT");

            return node;
        }
    }
}
