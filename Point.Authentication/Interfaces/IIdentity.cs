namespace Point.Authentication.Interfaces;

public interface IIdentity
{
    string Identifier();

    IDictionary<string, string> Data();
}