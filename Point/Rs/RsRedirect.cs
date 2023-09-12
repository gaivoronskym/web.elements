using System.Net;

namespace Point.Rs;

public sealed class RsRedirect : RsWrap
{
    public RsRedirect(string location)
        : this(location, HttpStatusCode.SeeOther)
    {
        
    }
    
    public RsRedirect(string location, HttpStatusCode code)
        : base(
                new RsWithHeader(
                        new RsWithStatus(code),
                        "Location", location
                    )
            )
    {
    }
}