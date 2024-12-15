namespace Point.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string val)
    {
        return val == string.Empty;
    }
}