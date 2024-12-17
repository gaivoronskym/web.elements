using Web.Elements.Rq;
using Web.Elements.Rs;

namespace Web.Elements.Pg;

public interface IPage
{
    Task<IResponse> Act(IRequest req);
}