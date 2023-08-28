namespace Point.Bind;

public interface IBranch
{
    IResponse? Route(IRequest req);
}