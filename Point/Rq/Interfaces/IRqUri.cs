namespace Point.Rq.Interfaces;

public interface IRqUri : IRequest
{
    Uri Uri();
    
    IDictionary<string, string> Query();
}