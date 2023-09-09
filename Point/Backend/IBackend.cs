namespace Point.Backend;

public interface IBackend
{
    Task StartAsync();

    void Stop();
}