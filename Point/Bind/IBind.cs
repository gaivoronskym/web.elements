namespace Point.Bind;

public interface IBind
{
    IResponse? Route(IRequest req);
}