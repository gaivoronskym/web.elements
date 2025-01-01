using System.Net;
using System.Xml.Linq;

namespace Web.Elements.Rs;

public sealed class RsXml : RsWrap
{
    public RsXml(string text)
        : this(
            () => new RsWithBody(
                XDocument.Parse(text).ToString()
            )
        )
    {
    }


    public RsXml(XNode node)
        : this(
            () => new RsWithBody(
                node.ToString()
            )
        )
    {
    }

    public RsXml(IResponse origin)
        : this(() => origin)
    {
    }
    
    public RsXml(Func<IResponse> origin)
        : base(
            new RsWithType(
                new RsWithStatus(
                    origin.Invoke(),
                    HttpStatusCode.OK
                ),
                "application/xml"
            )
        )
    {
    }
}