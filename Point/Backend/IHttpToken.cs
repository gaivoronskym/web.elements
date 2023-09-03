namespace Point.Backend;

public interface IBufferedRequest
{
    string Token(char delimiter);
    
    IBufferedRequest WithSkipped(int bytes);
}