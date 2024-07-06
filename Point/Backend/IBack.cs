namespace Point.Backend;

public interface IBack
{
    Task StartAsync();

    void Stop();
}