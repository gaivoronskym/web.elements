using Point;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;

namespace CustomServer;

public sealed class PtBookHtml : IPoint
{
    public IResponse Act(IRequest req)
    {
        return new RsHtml("<h1>Title: Object thinking</h1>");
    }
}