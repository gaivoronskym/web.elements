namespace Point;

public interface IIdentity
{
    string Identifier();

    IDictionary<string, string> Properties();
}