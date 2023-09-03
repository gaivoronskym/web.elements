using Point.Rq;
using Point.Rq.Interfaces;

namespace Point.Branch;

public class BranchTypes : IBranch
{
    private readonly string _type;
    private readonly IResponse _response;
    private const string AcceptAll = "*/*";
    
    public BranchTypes(string type, IResponse response)
    {
        _type = type;
        _response = response;
    }

    public IResponse? Route(IRequest req)
    {
        var acceptHeader = new RqHeaders(req).Headers()["Accept"] ?? "text/html";

        if (acceptHeader.Equals(AcceptAll))
        {
            return _response;
        }
        
        if (acceptHeader.Contains(_type))
        {
            return _response;
        }

        return default;
    }
}