namespace Web.Elements.Rq;

public interface IRqHeaders : IRequest
{
    IList<string> Header(string name);
    
    IList<string> Names();

    
}