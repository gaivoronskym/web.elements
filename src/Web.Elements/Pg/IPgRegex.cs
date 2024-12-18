using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Pg;

public interface IPgRegex
{
    Task<IResponse> Act(IRqRegex req);
}