namespace Point.Fk;

public interface IBind
{
    IResponse? Route(IRequest req);
}