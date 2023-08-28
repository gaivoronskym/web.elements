namespace Point.Bind;

public class BunchPool : IBunch
{
    private readonly IList<IBunch> _binds;

    public BunchPool(IList<IBunch> binds)
    {
        _binds = binds;
    }

    public IResponse? Route(IRequest req)
    {
        foreach (var fork in _binds)
        {
            IResponse? response = fork.Route(req);

            if (response is not null)
            {
                return response;
            }
        }

        return default;
    }
}