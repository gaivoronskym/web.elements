using Yaapii.Atoms;
using Yaapii.Atoms.Number;
using Yaapii.Atoms.Time;

namespace Point;

public sealed class QuerySet : Dictionary<string, string>, IQuerySet
{
    public string AsString(string key)
    {
        return this[key];
    }

    public DateTime AsDate(string key)
    {
        return new DateOf(this[key]).Value();
    }

    public INumber AsNumber(string key)
    {
        return new NumberOf(this[key]);
    }
}