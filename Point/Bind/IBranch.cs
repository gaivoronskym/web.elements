namespace Point.Bind;

public interface IBunch
{
    IResponse? Route(IRequest req);
}