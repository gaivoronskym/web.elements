namespace Point.Rs;

public class RsWithType : RsWrap
{
    private const string Header = "Content-Type";
    private const string Charset = "chartset";
    
    public RsWithType(IResponse origin, string type) 
        : this(origin, type, string.Empty)
    {
    }
    
    public RsWithType(IResponse origin, string type, string chartSet) 
        : base(Join(origin, type, chartSet))
    {
    }

    private static IResponse Join(IResponse res, string type, string charset)
    {
        if (!string.IsNullOrEmpty(charset))
        {
            return new RsWithHeader(
                    res,
                    Header,
                    $"{type}; {Charset}={charset}"
                );
        }

        return new RsWithHeader(
            res,
            Header,
            type
        );
    }
}