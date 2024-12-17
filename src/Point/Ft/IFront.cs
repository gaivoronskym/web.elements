using Point.Http;

namespace Point.Ft;

public interface IFront
{
    Task StartAsync(IExit exit);
}