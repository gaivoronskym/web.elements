namespace Point.Rq.Interfaces;

public interface IRqMultipart : IRequest
{
    IEnumerable<IRequest> Part(string name);
    
    IEnumerable<string> Names();
}