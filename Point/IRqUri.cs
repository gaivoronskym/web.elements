namespace Point;

public interface IRqUri : IRequest
{
    Uri Uri();

    IDictionary<string, object> RouteParams();
}