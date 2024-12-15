namespace Point.Http;

public interface IFront
{
    Task StartAsync(IExit exit);
}