using Point.Rq.Interfaces;

namespace Point.Authentication.Rq.Interfaces;

public interface IRqCookies : IRequest
{
    string Cookie(string key);

    IEnumerable<string> Names();
}