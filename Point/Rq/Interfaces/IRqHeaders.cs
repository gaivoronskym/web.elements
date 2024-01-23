namespace Point.Rq.Interfaces;

public interface IRqHeaders : IRequest
{
    IDictionary<string, string> Header(string name);
    
    IDictionary<string, string> Headers();
}