using System.Text.Json.Nodes;

namespace Point;

public interface IToken
{
    JsonObject Json();

    byte[] Encoded();
}