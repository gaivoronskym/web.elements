using System.Text.RegularExpressions;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Web.Elements.Fk;

public sealed class MediaType : IComparable<MediaType>
{
    private static readonly Regex NonDigits = new Regex("[^0-9\\.]", RegexOptions.Compiled);
    
    private readonly double prio;
    private readonly string high;
    private readonly string low;

    public MediaType(string text)
    {
        this.prio = MediaType.Priority(text);
        this.high = MediaType.HighPart(text);
        this.low = MediaType.LowPart(text);
    }
    
    public int CompareTo(MediaType? type)
    {
        var cmp = this.prio.CompareTo(type!.prio);

        if (cmp == 0)
        {
            cmp = string.Compare(this.high, type!.high, StringComparison.Ordinal);
            if (cmp == 0)
            {
                cmp = String.Compare(this.low, type!.low, StringComparison.Ordinal);
            }
        }
        
        return cmp;
    }

    public bool Matches(MediaType type)
    {
        const string star = "*";

        return (this.high.Equals(star)
                || type.high.Equals(star)
                || this.high.Equals(type.high))
               && (this.low.Equals(star)
                   || type.low.Equals(star)
                   || this.low.Equals(type.low));
    }

    private static string[] Split(string text)
    {
        return text.Split(';', 2);
    }

    private static double Priority(string text)
    {
        var parts = MediaType.Split(text);
        double priority;
        if (parts.Length > 1)
        {
            var num = MediaType.NonDigits.Replace(parts[1], string.Empty);
            if (string.IsNullOrEmpty(num))
            {
                priority = 0.0d;
            }
            else
            {
                priority = double.Parse(num);
            }
        }
        else
        {
            priority = 1.0d;
        }
        
        return priority;
    }

    private static string HighPart(string text)
    {
        return MediaType.Sectors(text)[0];
    }

    public static string LowPart(string text)
    {
        var sectors = MediaType.Sectors(text);
        IText sector;
        if (sectors.Length > 1)
        {
            sector = new Trimmed(new TextOf(sectors[1]));
        }
        else
        {
            sector = new TextOf("");
        }
        
        return sector.AsString();
    }

    private static string[] Sectors(string text)
    {
        return new Lower(
            new TextOf(MediaType.Split(text)[0])
        ).AsString().Split('/', 2);
    }
}