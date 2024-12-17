using System.Text.Json.Nodes;

namespace Web.Elements;

public interface IToken
{
    JsonObject Json();

    byte[] Encoded();
}