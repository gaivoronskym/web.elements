namespace Web.Elements.Rs;

public sealed class RsWithType : RsWrap
{
    private const string header = "Content-Type";
    private const string charset = "chartset";
    
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
                header,
                $"{type}; {RsWithType.charset}={charset}"
            );
        }

        return new RsWithHeader(
            res,
            header,
            type
        );
    }
}