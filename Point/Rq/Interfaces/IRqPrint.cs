using Yaapii.Atoms;

namespace Point.Rq.Interfaces;

public interface IRqPrint : IRequest, IText
{
    void Print(Stream output);
    
    void PrintHead(Stream output);
    
    void PrintBody(Stream output);
}