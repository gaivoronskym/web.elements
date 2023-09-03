namespace Point.Backend;

public interface IHttpToken
{
    string AsString(char delimiter);
    
    IHttpToken Skip(char delimiter);

    IHttpToken SkipNext(byte length);

    bool NextIs(string token);
}