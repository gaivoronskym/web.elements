namespace Point.Rq;

public interface IRqCookies : IRequest
{
    string Cookie(string key);

    IEnumerable<string> Names();
}