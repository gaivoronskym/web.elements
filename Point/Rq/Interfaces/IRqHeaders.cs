namespace Point.Rq.Interfaces;

public interface IRqHeaders : IRequest
{
    IDictionary<string, string> Headers();
}