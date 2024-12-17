namespace Web.Elements.Extensions;

public static class DateTimeExtensions
{
    public static string ToCookieDateFormat(this DateTime date)
    {
        return $"Expires={date:R}";
    }
}