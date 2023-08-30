namespace Point.Rq.Interfaces;

public interface IRqUri : IRequest
{
    Uri Uri();

    IDictionary<string, object> RouteParams();

    IDictionary<string, object> Query();
}