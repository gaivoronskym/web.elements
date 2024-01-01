using System.Text.Json.Nodes;

namespace Point.Authentication.Interfaces;

public interface IToken
{
    JsonObject Json();

    byte[] Encoded();
}