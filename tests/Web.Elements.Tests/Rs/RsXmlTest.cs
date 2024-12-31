using System.Xml.Linq;
using Web.Elements.Rs;
using Yaapii.Atoms.Text;

namespace Web.Elements.Tests.Rs;

public class RsXmlTest
{
    [Fact]
    public void MakesJsonResponse()
    {
        var xml = XDocument.Parse("<root><inner>Hello, world</inner></root>").ToString();

        Assert.Equal(
            new Joined(
                Environment.NewLine,
                "HTTP/1.1 200 OK",
                $"Content-Length: {xml.Length}",
                "Content-Type: application/xml",
                "",
                xml
            ).AsString(),
            new RsPrint(
                new RsXml(
                    xml
                )
            ).AsString()
        );
    }
}