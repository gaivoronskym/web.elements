namespace Point.Bind;

public class BunchWrap : IBunch
{
    private readonly IBunch _origin;

    public BunchWrap(IBunch origin)
    {
        _origin = origin;
    }

    public IResponse? Route(IRequest req)
    {
        return _origin.Route(req);
    }
}