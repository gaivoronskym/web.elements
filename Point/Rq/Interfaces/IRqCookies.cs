namespace Point.Rq.Interfaces;

public interface IRqCookies : IRequest
{
    string Cookie(string key);

    IEnumerable<string> Names();
}