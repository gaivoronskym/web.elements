namespace Point.Bind;

public class BdChain : IBind
{
    private readonly IList<IBind> _forks;

    public BdChain(IList<IBind> forks)
    {
        _forks = forks;
    }

    public IResponse? Route(IRequest req)
    {
        foreach (var fork in _forks)
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