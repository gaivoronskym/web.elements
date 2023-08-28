namespace Point.Bind;

public class BindChain : IBind
{
    private readonly IList<IBind> _binds;

    public BindChain(IList<IBind> binds)
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