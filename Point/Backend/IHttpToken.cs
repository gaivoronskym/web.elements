namespace Point.Backend;

public interface IHttpToken
{
    string AsString(char delimiter);

    Stream Stream();
    
    IHttpToken Skip(char delimiter);

    IHttpToken SkipNext(byte length);

    bool NextIs(string token);
}