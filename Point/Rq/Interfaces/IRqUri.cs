namespace Point.Rq.Interfaces;

public interface IRqUri : IRequest
{
    Uri Uri();

    IQuerySet Route();

    IQuerySet Query();
}