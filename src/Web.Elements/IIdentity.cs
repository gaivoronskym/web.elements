namespace Web.Elements;

public interface IIdentity
{
    string Identifier();

    IDictionary<string, string> Properties();
}