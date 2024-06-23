using Yaapii.Atoms;

namespace Point;

public interface IQuerySet : IDictionary<string, string>
{
    string AsString(string key);

    DateTime AsDate(string key);

    INumber AsNumber(string key);
}